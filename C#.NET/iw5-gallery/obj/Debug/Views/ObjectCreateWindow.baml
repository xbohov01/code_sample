   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:iw5_gallery.Viewsiw5_gallery.Views  5   	   ?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5   	   *    $iw5_gallery.Views.ObjectCreateWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:iw5_gallery.Views     5   	   ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5   	     � Title$  
Add Object��5   	   $��90.593��6   $��305.644��6,    � DataContext 5	   	   �� *ObjectDetailViewModel+   ViewModelLocator �� Source# [  .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $
��Auto �6   6.   .+�5   
   o� $��0��6   .@�5      m� $
��Save��6    m� Command 6&   �� *SaveObjectCommand+ m� CommandParameter 6L   �� *Model+6q   5      �� 5   
   $��Object name��6   $��1��6&   $��0��63   6E   �� 5   
   $��1��6   $��1��6   $	��218��6/   ��6;   ��  �� Path$ 
Model.Name� �� UpdateSourceTrigger$ PropertyChanged=�6�   5      5      