   M S B A M L   `   `   `  ����F  BTestTaskIBA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TestTaskIBATestTaskIBA  5   	   %    TestTaskIBA.ExportingDataWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TestTaskIBA     5   	     � Title$  ExportingDataWindow��5   	   $	��450��6%   $	��800��62   .��5	      ��  �� Orientation$ 
Horizontal=�6   ���#0772A1.+�5
   
   �� ���#63AFD0.+�5      ��  �� TextAlignment$ Center=�6   	Датa67   (  "System.Windows.Controls.DatePicker  5      -   5        $��DatePickerEmployee��5         SelectedDateFormat$ Short=�63      IsTodayHighlighted . 6k   �� 5      $ Center=�6   Имя66   �� 5      -   5        $��EmployeeFirstName��5      61   �� 5      $ Center=�6   Фамилия6:   �� 5      -   5        $��EmployeeLastName��5      60   �� 5      $ Center=�6   
Username6;   �� 5      -   5        $��EmployeeUserName��5      60   �� 5      $ Center=�6   
Город68   �� 5      -   5        $��EmployeeCity��5      6,   �� 5      $ Center=�6   Страна69   �� 5      -   5        $��EmployeeCountry��5      6/   � 5       � ColumnDefinitions 5      �� 5      6)   �� 5      6)   .+�5      �� -   5        $��Filter_Accept_Button��5   -   $��Применить��6   $��0��6l   �� 5      -   5        $��Filter_Clear_Button��5   ,   $��Отменить��6   $��1��6i   5      5      �� 5    
   .+�5!      &   System.Windows.Controls.DataGrid  -	   5          Name$ EmployeesGrid��5!   3      AutoGenerateColumns .     
IsReadOnly . $��Visible=�6\   $	��300��6�   	   Columns	 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Дата��6R   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   �� *Date+ �� StringFormat$ d��6R     5$      $
 Имя��6G    6)   �� *	FirstName+6G     5%      $
 Фамилия��6F    6)   �� *
LastName+6F     5&      $
 Username��6F    6)   �� *
UserName+6F     5'      $
 
Город��6B    6)   �� *City+6B     5(      $
 Страна��6E    6)   �� *	Country+6E   5*      � 5+       5,      �� 5-      6)   �� 5.      6)   .+�50      �� -
   5        $��XML_Export_Button��50   1   $��Экспорт в XML��6   $��0��6j   �� 51      -   5        $��Excel_Export_Button��51   3   $��Экспорт в Excel��6   $��1��6p   52      53      54      55      