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
    public partial class AddTaskWindow : Window
    {
        public AddTodoTaskDto NewTask { get; set; }
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            NewTask = new AddTodoTaskDto
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
