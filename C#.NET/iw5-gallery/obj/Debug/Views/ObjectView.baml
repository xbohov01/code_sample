   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5      5clr-namespace:iw5_gallery.Viewsiw5_gallery.Views  5      "    iw5_gallery.Views.ObjectView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      +viewsclr-namespace:iw5_gallery.Views  5        � DataContext  5
      �� *ObjectDetailViewModel+   ViewModelLocator �� Source# [  .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $
��Auto �6   6.    � RowDefinitions 5   
   �� 5      $
��Auto �6   $��20��6*   6;   �� 5      $
��Auto �6   $	��580��6*   6<   .+�5   
   o� $��0��6   $��0��6   .@�5      m� $��Delete��6    m� Command 6'   �� *DeleteObjectCommand+m� 5      $��Untag��6    6&   �� *UntagObjectCommand+5      � 5   
   $��1��6   $��0��6   $��0,2,0,0q�6,    5      �� 5      $
��Auto �6#   62   �� 5      $
��Auto �6#   62    5      �� 5       $
��Auto �6    60   �� 5!      $
��Auto �6    60   .+�5#      �� $��Object Name��6   $��0��6*   $��0��67   6I   �� 5$      $��1��6   $��0��6&   ��63   �� *
Model.Name+6Q   �� 5%      -   5         ��Name$ ObjectPhotosListBox��5%   3   $��1��6   $��1��6#   $��457.5��6z   $	��130��6�   ��6P   ��  �� Path$ Model.Photos�.  (System.Windows.Interactivity.Interaction   Triggers 5&      /  )System.Windows.Interactivity.EventTrigger  5'         	EventName$ SelectionChanged��6%   .  (System.Windows.Interactivity.TriggerBase	   Actions.	 5(      6  0System.Windows.Interactivity.InvokeCommandAction  
   Command
 60   �� *SelectObjectPhotoCommand+   CommandParameter 5)   0   ��  �� ElementName$ ObjectPhotosListBox��$ SelectedItem�5*      ��5,      �� 5-      .R�5.      ��  �� Orientation$ Vertical=�6%   .+�5/      �� ��6(   �� $
 Name�6E   �� 50      $
��Left=�6v   $��55��6�   $��70��6�   $	��Top=�6�   ��6$   �� $ Image�  ImageToSourceConverter �� 	Converter# [ 6�   51      52      54      55      .   (iw5_gallery.Views.ImageDetailDefaultView  56   
   $��0��6'   $��2��64   $��1��6E   $��
0,0,-0.2,0q�6U   6k   57      58      