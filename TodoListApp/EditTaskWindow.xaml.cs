using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TodoList.Shared.Models;

namespace TodoListApp
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        public UpdateTodoTaskDto UpdatedTask { get; set; }
        private readonly TodoTask originalTask;
        public EditTaskWindow(TodoTask task)
        {
            InitializeComponent();
            originalTask = task;

            TitleInput.Text = task.Title;
            DescriptionInput.Text = task.Description;
            DeadlineInput.SelectedDate = task.Deadline;
            PriorityInput.SelectedItem = PriorityInput.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(i => i.Content.ToString() == task.Priority.ToString());
            CompletionInput.IsChecked = task.Completion;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            int selectedPriority = int.Parse((PriorityInput.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "1");

            UpdatedTask = new UpdateTodoTaskDto
            {
                Title = TitleInput.Text,
                Description = DescriptionInput.Text,
                Deadline = DeadlineInput.SelectedDate ?? DateTime.Today,
                Priority = int.Parse((PriorityInput.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "1"),
                Completion = CompletionInput.IsChecked ?? false
            };

            DialogResult = true;
            Close();
        }
    }
}
