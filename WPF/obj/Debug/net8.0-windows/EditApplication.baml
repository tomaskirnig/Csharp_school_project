   M S B A M L   `   `   `  ����>  :WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPFWPF  5   	       WPF.EditApplication   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:WPF     5   	     � Title$  EditApplication��5   	   $	��450��6!   $	��800��6.   .��5	      � $��Center=�6   $��Center=�6&    � ShowGridLines$ False��6C   $	��800��6Y   $	��450��6e    � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��0��6   $��0��6   $��
Highschool��6-   $��Center=�6B   $��Center=�6_   �� 5   
   -   5         ��Name$ 	cmbSchool��5   0   $��0��6   $��1��6    $��45,20,45,15q�6C   �� 5   
   $��1��6   $��0��6   $��Specialization 1��6-   $��Center=�6H   $��Center=�6e   �� 5   
   -   5        $ cmbSpecialization1��5   0   $��1��6   $��1��6    $��45,20,45,15q�6L   �� 5   
   $��2��6   $��0��6   $��Specialization 2��6-   $��Center=�6H   $��Center=�6e   �� 5   
   -   5        $ cmbSpecialization2��5   0   $��2��6   $��1��6    $��45,20,45,15q�6L   �� 5!   
   $��3��6   $��0��6   $��Specialization 3��6-   $��Center=�6H   $��Center=�6e   �� 5"   
   -   5        $ cmbSpecialization3��5"   0   $��3��6   $��1��6    $��45,20,45,15q�6L   �� 5$   
   $��4��6   $��0��6   $��Student��6-   $��Center=�6?   $��Center=�6\   �� 5%   
   -   5        $ 
cmbStudent��5%   0   $��4��6   $��1��6    $��45,20,45,15q�6D   �� 5'   
   $��5��6   $��0��6   $��Date of creation��6-   $��Center=�6H   $��Center=�6e   �� 5(   
   -   5         ��Name$ txtDateOfCreation��5(   O   $��5��6   $��1��6   $��45,20,45,15q�6j   ��6/   �� *DateOfCreation+6j   �� 5*   
   -   5        $��6��5*      $��0��6   $
��Save��6.   $��155,10,155,30q�6W   �� 5+   
   -   5        $��6��5+      $��1��6   $��Cancel��6.   $��155,10,155,30q�6[   5,      5-      