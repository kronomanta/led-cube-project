   M S B A M L   `   `   `  ����H  DLedCubeClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:LedCubeClient.ViewLedCubeClient.View  5   	   E'clr-namespace:LedCubeClient.View.CommonLedCubeClient.View.Common  5   	   !    LedCubeClient.View.MainView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1Menu clr-namespace:LedCubeClient.View     5   	   :Common'clr-namespace:LedCubeClient.View.Common     5   	     � Title$  LedCube��5   	    � WindowStartupLocation$ CenterScreen=�6   $	��670��5   	   $	��390��6    � 
ResizeMode$ NoResize=�5	   	    � DataContext 5   	   �� *MainViewModel+   Locator �� Source# [  .��5      � $��0,0,0,10q�6    � 	Resources 5   
   4   .LedCubeClient.View.Common.EnumBooleanConverter  enumBooleanConverter��%   &	         5      6*   5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   !   LedCubeClient.View.MenuView  $��0��6   � 5   
   $��1��6   $��10,10,10,10q�6    � ColumnDefinitions 5      �� 5      $	��355 �6#   �� 5      $	��145 �6#   �� 5      $	��145 �6#    5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5"      �� -   5        $��MessageBoard��5"   W   $��0��6   $��0��6'   $��Serial Port Communication��64   .��5#      �� $	��230��6   $��Disabled=�5$      ��5#   (   �� *CommunicationMessages+��5%      �� 5&      .R�5'      ]� .+�5(   "   �� $
��Wrap=�6,   ��6A   ��  �� BindsDirectlyToSource . 5)      5*      5,      5-      �� 5.      -   5        $��	groupBox1��5/      $��1��5.      $��Comm. Options��6(   $��Stretch=�6?   $	��Top=�5/   *   .��50      �� .+�51      �� $
��Port��6   �� 52      -   5        $��cbPort��52      	 �� SelectedIndex$	 0��6q   
 �� SelectedItem
 6-   �� *
PortName+��6O   �� *	PortNames+6q   �� 53      $��	Baud rate��6   �� 54      -   5        $��cbBaud��54      $	 5��6v   
 6-   �� *
BaudRate+��6O   �� *BaudRateValues+6v   �� 55      $��Parity��6   �� 56      -   5        $��cbParity��56      $	 0��6t   
 6/   �� *Parity+��6O   �� *ParityValues+6t   �� 57      $��	Stop bits��6   �� 58      -   5        $��
cbStopBits��58      $	 1��6x   
 61   �� *	StopBit+��6R   �� *StopBitValues+6x   �� 59      $��	Data bits��6   �� 5:      -   5        $��
cbDataBits��5:      $	 1��6x   
 61   �� *	DataBit+��6R   �� *DataBitValues+6x   5;      5<      �� 5=      $��2��6   .+�5>      �� $��Message Type��6<   $��Stretch=�6R   {�6   �� *
IsPortOpen+.��5?      �� .+�5@      	�  d� 	IsChecked 6&   ��  �� Path$ MessageType� �� 	Converter# [  �� ConverterParameter$ 
ToRealTime��.��6�   	Real time6�   	� 5A       6&   �� $ MessageType�# [ $ ToMemory��.��6�   Memory6�   	� 5B       6&   �� $ MessageType�# [ $ Control��.��6�   	Control6�   	� 5C       6&   �� $ MessageType�# [ $ Reserved��.��6�   
Reserved6�   5D      5E      �� 5F      $��Control Type��6P   $��Stretch=�6f   {�6   �� * IsControlTypeGroupBoxIsEnabled+.��5G      �� .+�5H      	�  6&   �� $ ControlType�# [ $ Eeprom��.��6�   	To EEPROM6�   	� 5I       6&   �� $ ControlType�# [ $ Pc��.��6�   	From PC6�   5J      5K      5L      5M      ]� 5N   
   $��0��6   $��2��6$   ��.  $��	10,0,10,0q�6G   $��Stretch=�6[   .+�5O      �� $
��Left=�6g   ��6   �� *ConnectionButtonText+��6>   �� *OpenCloseConnCommand+6g   �� 5P      $��Set control��6   $��5,0,5,0q�6�   ��6+   �� *SendControlCommand+{�6R   �� * IsControlTypeGroupBoxIsEnabled+6�   ]� 5Q      -   5        $��FrameController��5Q      ��.  {�6/   �� *IsFrameControllerEnabled+.+�5R      �� $��<<��6   $��5,0,5,0q�6R   ��6,   �� *FrameFirstCommand+6R   �� 5S      $��<��6   $��0,0,5,0q�6M   ��6(   �� *FramePrevCommand+6M   �� 5T      $
��Play��6   $��0,0,5,0q�6M   ��6(   �� *FramePlayCommand+6M   �� 5U      $
��Stop��6   $��0,0,5,0q�6M   ��6(   �� *FrameStopCommand+6M   �� 5V      $��>��6   $��0,0,5,0q�6M   ��6(   �� *FrameNextCommand+6M   �� 5W      $��>>��6   $��0,0,5,0q�6Q   ��6,   �� *FrameLastCommand+6Q   �� 5X      $��Send one frame��6   $��0,0,5,0q�6W   ��62   �� *SendFrameCommand+6W   �� 5Y      $��Repeat animation��6   $��16��6]   $	��114��6i   ��66   �� *FrameRepeatCommand+6i   5Z      �� 5[      $��40��69   $��Right=�6D   {�.   �� TextAlignment$ Right=�6m   
���������6   �� *CurrentAllFrame+6�   �� 5\      $��Current/All: ��6   $��Right=�6,   5]      5^      5_      