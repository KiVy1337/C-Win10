   M S B A M L   `   `   `  ����F  BTestTaskIBA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TestTaskIBATestTaskIBA  5   	   #    TestTaskIBA.AddEmployeeWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TestTaskIBA     5   	     � Title$-  'Добавление работника��5   	   $	��500��6&   $	��500��63   $��25(�6?    � WindowStartupLocation$ CenterOwner=�6M   .��5	      �� ���#024A68.+�5
   
   ��  �� TextAlignment$ Center=�6   Выберите Дату6<   (  "System.Windows.Controls.DatePicker  5   
   -   5        $��DatePickerAddEmployee��5         SelectedDateFormat$ Short=�62      IsTodayHighlighted . 6j   �� 5   
   $ Center=�6   Напишите имя6;   �� 5   
   -   5        $��EmployeeFirstName��5      6-   �� 5   
   $ Center=�6   !Напишите фамилию6?   �� 5   
   -   5        $��EmployeeLastName��5      6,   �� 5   
   $ Center=�6   
Username67   �� 5   
   -   5        $��EmployeeUserName��5      6,   �� 5   
   $ Center=�6   Напишите город6=   �� 5   
   -   5        $��EmployeeCity��5      6(   �� 5   
   $ Center=�6   Напишите страну6>   �� 5   
   -   5        $��EmployeeCountry��5      6+   � 5   
    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   .+�5      �� -   5        $��	AddButton��5      $��0��6&    �� 	IsDefault . .��6`   Добавить6j   �� 5      -   5        $��CancelButton��5      $��1��6)    �� IsCancel . .��6J   Отмена6R   5      5      5      