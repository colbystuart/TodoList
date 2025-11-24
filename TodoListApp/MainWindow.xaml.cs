using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TodoList.Shared.Models;

namespace TodoListApp
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private List<TodoTask> _allTasks = new();

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5223") };
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTodos();
        }

        private async Task LoadTodos()
        {
            try
            {
                var todos = await _httpClient.GetFromJsonAsync<List<TodoTask>>("api/TodoList");
                if (todos != null)
                {
                    _allTasks = todos;
                    TodoListBox.ItemsSource = _allTasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TodoCalendar.SelectedDate == null) return;

            DateTime selectedDate = TodoCalendar.SelectedDate.Value.Date;
            var dailyTasks = _allTasks.Where(t => t.Deadline.Date == selectedDate).ToList();

            if (dailyTasks.Count == 0)
            {
                TodoListBox.ItemsSource = null;
                TodoListDescription.ItemsSource = null;
            }
            else
            {
                TodoListBox.ItemsSource = dailyTasks;
                TodoListDescription.ItemsSource = dailyTasks;
            }

        }

        private async void AddTaskButtonPopUp_Click(object sender, RoutedEventArgs e)
        {
            var popup = new AddTaskWindow { Owner = this };
            if (popup.ShowDialog() == true) 
            {
                var newTask = popup.NewTask;
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5223");

                var response = await client.PostAsJsonAsync("/api/TodoList", newTask);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Task Added");
                    await LoadTodos();
                }
                else
                {
                    MessageBox.Show($"Failed to add task: {response.StatusCode}");
                }
            }
        }

        private async void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TodoTask;
            if (task == null) return;

            var editWindow = new EditTaskWindow(task) { Owner = this };
            if (editWindow.ShowDialog() == true)
            {
                var updatedTask = editWindow.UpdatedTask;

                using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5223") };
                var response = await client.PutAsJsonAsync($"api/TodoList/{task.Id}", updatedTask);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Task updated.");
                    await LoadTodos();
                }
                else
                {
                    MessageBox.Show($"Failed to update task: {response.StatusCode}");
                }
            }
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TodoTask;
            if (task == null) return;

            var confirm = MessageBox.Show($"Delete task '{task.Title}'?", "Confirm", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5223") };
                var response = await client.DeleteAsync($"api/TodoList/{task.Id}");

                if (response.IsSuccessStatusCode) 
                {
                    MessageBox.Show("Task Deleted.");
                    await LoadTodos();
                }
                else
                {
                    MessageBox.Show($"Failed to delete task: {response.StatusCode}");
                }
            }
        }

        private void ReturnHome_Click(object sender, RoutedEventArgs e)
        {
            TodoCalendar.SelectedDate = null;

            TodoListBox.ItemsSource = _allTasks;
        }

        private void TodoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = TodoListBox.SelectedItem as TodoTask;
            if (selectedTask != null)
            {
                TodoListDescription.ItemsSource = new List<TodoTask> { selectedTask };
            }
            else
            {
                TodoListDescription.ItemsSource = null;
            }
        }

        private async void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TodoTask;
            if (task == null) return;

            // Build updated DTO with Completion = true
            var updatedTask = new UpdateTodoTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                Priority = task.Priority,
                Completion = true
            };

            using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5223") };
            var response = await client.PutAsJsonAsync($"api/TodoList/{task.Id}", updatedTask);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Task '{task.Title}' marked complete.");
                await LoadTodos(); // refresh UI
            }
            else
            {
                MessageBox.Show($"Failed to mark task complete: {response.StatusCode}");
            }
        }
    }

    public class TodoTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public bool Completion { get; set; }
    }
}
