   M S B A M L   `   `   `  ����>  :WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPFWPF  5   	       WPF.EditSpecialization   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:WPF     5   	     � Title$  EditSpecialization��5   	   $	��450��6$   $	��800��61   .��5	      � $��Center=�6   $��Center=�6&    � ShowGridLines$ False��6C   $	��800��6Y   $	��450��6e    � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��0��6   $��0��6   $��
Highschool��6-   $��Center=�6B   $��Center=�6_   �� 5   
   -   5         ��Name$ cbmHighschool��5   0   $��0��6   $��1��6    $��30,20,55,15q�6G   �� 5   
   $��1��6   $��0��6   $
��Name��6-   $��Center=�6<   $��Center=�6Y   �� 5   
   -   5         ��Name$ txtName��5   E   $��1��6   $��1��6   $��30,20,55,15q�6V   ��6/   �� *Name+6V   �� 5   
   $��2��6   $��0��6   $��Description��6-   $��Center=�6C   $��Center=�6`   �� 5    
   -   5        $ txtDescription��5    L   $��2��6   $��1��6   $��30,20,55,15q�6d   ��6/   �� *Description+6d   �� 5"   
   $��3��6   $��0��6   $#��Number of available positions��6-   $��Center=�6U   $��Center=�6r   �� 5#   
   -   5        $&  txtNumber_Of_Available_Positions��5#   ^   $��3��6   $��1��6   $��30,20,55,15q�6�   ��6/   �� *Number_Of_Available_Positions+6�   �� 5%   
   $��4��6   $��0��6   $ ��Duration of specialization��6-   $��Center=�6R   $��Center=�6o   �� 5&   
   -   5        $# txtDuration_Of_Specialization��5&   [   $��4��6   $��1��6   $��30,20,55,15q�6�   ��6/   �� *Duration_Of_Specialization+6�   �� 5(   
   $��5��6   $��0��6   $��Language��6-   $��Center=�6@   $��Center=�6]   �� 5)   
   -   5        $ txtLanguage��5)   I   $��5��6   $��1��6   $��30,20,55,15q�6^   ��6/   �� *
Language+6^   �� 5+   
   $��6��6   $��0��6   $��Form of study��6-   $��Center=�6E   $��Center=�6b   �� 5,   
   -   5        $ txtForm_Of_Study��5,   N   $��6��6   $��1��6   $��30,20,55,15q�6h   ��6/   �� *Form_Of_Study+6h   �� 5.   
   -   5        $��7��5.      $��0��6   $
��Save��6.   $��155,15,155,20q�6W   �� 5/   
   -	   5        $��7��5/      $��1��6   $��Cancel��6.   $��155,15,155,20q�6[   50      51      