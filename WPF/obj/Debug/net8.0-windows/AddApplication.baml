   M S B A M L   `   `   `  ����>  :WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPFWPF  5   	       WPF.AddApplication   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:WPF     5   	     � Title$  AddApplication��5   	   $	��450��6    $	��800��6-   .��5	      � $��Center=�6   $��Center=�6&    � ShowGridLines$ False��6C   $	��800��6Y   $	��450��6e    � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��0��6   $��0��6   $��School��6-   $��Center=�6>   $��Center=�6[   �� 5   
   -   5         ��Name$ 	cmbSchool��5   �   $��0��6f   $��1��6s   $��45,25,45,25q�6�   ��6   �� *cbItemsSchools+ �� SelectedItem 6:   �� *SelectedItemSchool+6�   �� 5   
   $��1��6   $��0��6   $��Specialization 1��6-   $��Center=�6H   $��Center=�6e   �� 5   
   -   5        $ cmbSpecialization1��5   �   $��1��6w   $��1��6�   $��45,25,45,25q�6�   ��6   �� *cbItemsSpecializations+ 6B   �� *SelectedItemSpecialization1+6�   �� 5   
   $��2��6   $��0��6   $��Specialization 2��6-   $��Center=�6H   $��Center=�6e   �� 5   
   -   5        $ cmbSpecialization2��5   �   $��2��6x   $��1��6�   $��45,25,45,25q�6�   ��6   �� *cbItemsSpecializations+ 6C   �� *SelectedItemSpecialization2+6�   �� 5    
   $��3��6   $��0��6   $��Specialization 3��6-   $��Center=�6H   $��Center=�6e   �� 5!   
   -   5        $ cmbSpecialization3��5!   �   $��3��6w   $��1��6�   $��45,25,45,25q�6�   ��6   �� *cbItemsSpecializations+ 6B   �� *SelectedItemSpecialization3+6�   �� 5#   
   $��4��6   $��0��6   $��Student��6-   $��Center=�6?   $��Center=�6\   �� 5$   
   -   5        $ 
cmbStudent��5$   ~   $��4��6a   $��1��6n   $��45,25,45,25q�6�   ��6   �� *cbItemsStudents+ 6;   �� *SelectedItem+6�   �� 5&   
   -   5        $��5��5&      $��0��6   $	��Add��6.   $��155,25,155,25q�6U   �� 5'   
   -   5        $��5��5'      $��1��6   $��Cancel��6.   $��155,25,155,25q�6[   5(      5)      