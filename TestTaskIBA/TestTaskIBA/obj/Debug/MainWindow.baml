   M S B A M L   `   `   `  ����F  BTestTaskIBA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TestTaskIBATestTaskIBA  5   	       TestTaskIBA.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TestTaskIBA     5   	   -   5          � Title$2  ,Информация о работниках��5   	   $	��500��6)   $
��1000��66   .��5	      ��  �� Orientation$ 
Horizontal=�6   ���#0772A1.+�5
   
   �� $��Stretch=�6   ���#63AFD0.+�5      �� -   5        $��
Add_Button��5   /   $#��Добавить запись��6   $	��150��6Z   $��30��6f   $��0 10 0 0q�6r   �� 5      -   5        $��Delete_Button��5   .   $!��Удалить записи��6   $	��150��6_   $��30��6k   �� 5      -   5        $��Update_Button��5   /   $#��Обновить запись��6   $	��150��6`   $��30��6l   �� 5      -   5        $��Export_Button��5   .   $!��Экспорт данных��6   {�.  $	��150��6q   $��30��6}   �� 5      -   5        $��LoadBarTextBlock��5   g    �� TextAlignment$ Center=�6   $/��)Загрузка данных (0 / 50000)��60   $��0 20 0 5q�6U   6�   � 5      -   5        $��LoadBar��5   J    � Minimum$ 0Q�6    � Maximum$	 500Q�6&    � Value$ 0Q�64   $��20��6>   6J   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ EmployeesGrid��5   /      AutoGenerateColumns .     
IsReadOnly . $	��300��6X   $	��Top=�6e   	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Дата��6N   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *Date+ �� StringFormat$ d��6N     5      $
 Имя��6C    6%   �� *	FirstName+6C     5      $
 Фамилия��6B    6%   �� *
LastName+6B     5      $
 Username��6B    6%   �� *
UserName+6B     5      $
 
Город��6>    6%   �� *City+6>     5      $
 Страна��6A    6%   �� *	Country+6A   5      5      5      