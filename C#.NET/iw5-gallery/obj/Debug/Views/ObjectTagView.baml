   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5      %    iw5_gallery.Views.ObjectTagView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5      $
��Auto��5
      $	��150��6     � DataContext  5      �� *ObjectDetailViewModel+   ViewModelLocator �� Source# [  .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      6!    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      6   .+�5   
   o� .@�5      m� $��Tag on photo��6    m� Command 6-   �� *	TagObject+5      � 5   
   $��1��6   $��0��6   $��2��6"    5      �� 5      $
��Auto �6#   62   �� 5      $��* �6#   6/    5      �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   .+�5       �� $��Object Name��6   $��0��6*   $��0��67   6I   �� 5!      $��1��6   ��6&   �� *
Model.Name+6D   5"      5#      5$      