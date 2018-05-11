# AiForms.Effects for Xamarin.Forms

AiForms.Effect は Android と iOS に特化することにより、標準のコントロールをより便利にするための機能を提供する Xamarin.Forms の Effectsライブラリです。

## 機能
* [AddTouch](#addtouch)
    * 各種タッチイベントを追加
* [SizeToFit](#sizetofit)
    * フォントサイズをLabelの大きさに調整
* [Border](#border)
    * 罫線の追加.
* [Placeholder](#placeholder)
	* Editor に Placeholder を追加.
* [ToFlatButton](#toflatbutton)
	* ボタンをフラットにする (Android)
* [AddText](#addtext)
	* 1行テキストを任意のviewに追加
* [AddCommand](#addcommand)
    * コマンドを任意のviewに追加
* [AddNumberPicker](#addnumberpicker)
    * NumberPicker 機能を任意のviewに追加
* [AddTimePicker](#addtimepicker)
	* TimePicker 機能を任意のviewに追加
* [AddDatePicker](#adddatepicker)
	* DatePicker 機能を任意のviewに追加
* [AlterLineHeight](#alterlineheight)
    * LabelとEditorの行の高さを変更
* [AlterColor](#altercolor)
	* 通常変えられない箇所の色を変える


## 動作条件など

iOS:iPhone5s,iPod touch6,iOS9.3  
Android:version 5.1.1 (only FormsAppcompatActivity) / API22

## Nuget インストール

```bash
Install-Package AiForms.Effects
```

共有プロジェクト（PCL / .NETStandard）と各プラットフォームのプロジェクトにそれぞれインストールする必要があります。

### iOS プロジェクト

iOSで使用するために、AppDelegate.csに以下のようなコードの追加が必要です。

```csharp
public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
    global::Xamarin.Forms.Forms.Init();

    AiForms.Effects.iOS.Effects.Init();  //need to write here

    LoadApplication(new App(new iOSInitializer()));

    return base.FinishedLaunching(app, options);
}
```

## AddTouch

この Effectは …（めんどくさくなったので翻訳一旦中止w）
This is the effect that adds touch events (begin, move, end, cancel) to a view.
Each touch events provides location property and can be taken X and Y position.

### Parameters

* On
    * Effect On / Off

### TouchRecognizer events

* TouchBegin
* TouchMove
* TouchEnd
* TouchCancel

### Demo

https://youtu.be/9zrVQcr_Oqo

### How to use

This effect usage is a little different from the other effects.
First of all, set On attached property to a target control and set the value to true in XAML.

```xml
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage 
    ...
    xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects">
    <StackLayout HeightRequest="300" ef:AddTouch.On="true" x:Name="container" />
</ContentPage>
```

In turn, use AddTouch.GetRecognizer method, get a TouchRecognizer in code.
This recognizer can be used to handle each touch events.

```cs
var recognizer = AddTouch.GetRecognizer(container);

recognizer.TouchBegin += (sender, e) => {
    Debug.WriteLine("TouchBegin");
};

recognizer.TouchMove += (sender, e) =>  {
    Debug.WriteLine("TouchMove");
    Debug.WriteLine($"X: {e.Location.X} Y:{e.Location.Y}"); 
};

recognizer.TouchEnd += (sender, e) => {
    Debug.WriteLine("TouchEnd");
};

recognizer.TouchCancel += (sender, e) => {
    Debug.WriteLine("TouchCancel");
};
```

## SizeToFit

This is the effect that make font size adjusted to fit the Label size.
This can be used only Label. 

### Parameters

* On
    * Effect On/Off (true is On)
* CanExpand
    * Whether font size is expanded when making it fit. (Default true)
    * If false, font size won't be expanded and only shrinked.

### Demo

https://youtu.be/yMjcFOp38XE

### How to write with Xaml

```xml
<ContentPage 
	xmlns="http://xamarin.com/schemas/2014/forms" 
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
	x:Class="AiEffects.TestApp.Views.BorderPage">
	<Label Text="LongText..." ef:SizeToFit.On="true" ef.SizeToFit.CanExpand="false"
			HeightRequest="50" Width="200"  />
</ContentPage>
```

## Border

This is the effect that add border to a view.  
Entry, Picker, DatePicker and TimePicker on iOS have a border by default.  
When specifying their width 0, it is possible that hide border.

<img src="images/border_ios.gif" /> <img src="images/border_droid.gif" />

### Parameters

* On
	* Effect On/Off (true is On)
* Width
	* Border width (default 0)
* Color
	* Border color (default transparent)
* Radius
	* Border radius (default 0)

### How to write with Xaml

```xml
<ContentPage 
	xmlns="http://xamarin.com/schemas/2014/forms" 
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
	x:Class="AiEffects.TestApp.Views.BorderPage">
	<StackLayout Margin="4" 
        ef:Border.Width="2" ef:Border.Radius="6" ef:Border.Color="Red">
		<Label Text="hoge" />
        <Label Text="fuga" />
	</StackLayout>
</ContentPage>
```

### Limitations

* On Android Entry, Picker, DatePicker, TimePicker's input underline is hidden if this effect attached.
* On Android, Button is not displayed correctly. Use [ToFlatButton](#toflatbutton) for button.
* On Android WebView, Frame, ScrollView are not supported.
* On Android ListView and TableView overflow background from border.
* Using AddCommand simultaneously is not supported.

## Placeholder

This is the effect that show placeholder on Editor.  
This effect supports Editor only.

<img src="images/placeholder_ios.gif" /> <img src="images/placeholder_droid.gif" />

### Parameters

* On
	* Effect On/Off (true is On)
* Text
	* Placeholder text.
* Color
	* Placeholder color.

### How to write with Xaml

```xml
<ContentPage 
	xmlns="http://xamarin.com/schemas/2014/forms" 
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
	x:Class="AiEffects.TestApp.Views.BorderPage">
	<Editor HeightRequest="150"
		ef:Placeholder.On="true"
		ef:Placeholder.Text="placeholder text"
		ef:Placeholder.Color="#E0E0E0"
	/>
</ContentPage>
```

## ToFlatButton

This is the effect that alter Button to flat(only Android).  
If this effect is used, you will be able to design like iOS's Button.  
And also this effect will enable BorderRadius, BorderWidth and BorderColor of default button properties to use by Android.

<img src="images/toflat1.png" height="400" /> <img src="images/toflat2.png" height="400" />

### Supported View

* Button (Android)

### Parameters

* On
    * Effect On/Off (true is On)
* RippleColor
	* Ripple effect color.(default none)


### How to write with Xaml

```xml
<Button Text="ButtonText" 
	ef:ToFlatButton.On="true" 
	ef:ToFlatButton.RippleColor="Red"
	BorderWidth="4" BorderColor="Green" BorderRadius="10" 
/>
```

## AddText

This is the effect that add one line text into a view.  
If you use this effect, for example you will be able to show a information that validations or character count etc.  
You will be able to change text position(top-left,top-right,bottom-left,bottom-right), text color,font size and margin by specifying property.

<img src="images/addtext_ios.gif" /> <img src="images/addtext_droid.gif" />

### Supported View

* Label
* Entry
* Editor
* StackLayout
* AbsoluteLayout

and more.

### Parameters

* On
	* Effect On/Off (true is On)
* Text
	* added text
* TextColor
	* Default Red
* BackgroundColor
	* BackgroundColor of inner text view.
	* Default Transparent
* FontSize
	* Default 8
* Margin
	* Distance from a side of target view to inner text view.
	* Default 0,0,0,0
* Padding
	* Padding of inner text view. 
	* Default 0,0,0,0
* HorizontalAlign
	* horizontal text position(Start or End). Default End.
* VerticalAlign
	* vertical text position(Start or End). Default Start.

### How to write with Xaml

```xml
<ContentPage 
	xmlns="http://xamarin.com/schemas/2014/forms" 
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
	x:Class="AiEffects.TestApp.Views.AddTextPage">
	<StackLayout Margin="4">
		<Entry HorizontalOptions="FillAndExpand" Text="{Binding Title}"
			ef:AddText.On="true" ef:AddText.TextColor="Red" 
			ef:AddText.FontSize="10" ef:AddText.Margin="4,8,4,8" 
			ef:AddText.Padding="2,4,2,4" ef:AddText.BackgroundColor="#A0F0F0E0"
			ef:AddText.HorizontalAlign="End"
			ef:AddText.VerticalAlign="Start" 
			ef:AddText.Text="{Binding TitleMessage}" />
	</StackLayout>
</ContentPage>
```

### Limitations

When device rotates, text position will not be right in case android.

## AddCommand

This Effect add Command function to a view.  
There are properties of Command and Parameter for tap and long tap.

### Supported View (in case Xamarin.Forms 2.3.4)

|                   | iOS  | Android |
| ----------------- | ---- | ------- |
| ActivityIndicator | ✅    | ✅       |
| BoxView           | ✅    | ✅       |
| Button            | ✅    | ✅       |
| DatePicker        | ❌    | ✅       |
| Editor            | ❌    | ❌       |
| Entry             | ❌    | ❌       |
| Image             | ✅    | ✅       |
| Label             | ✅    | ✅       |
| ListView          | ✅    | ❌       |
| Picker            | ❌    | ✅       |
| ProgressBar       | ✅    | ✅       |
| SearchBar         | ❌    | ❌       |
| Slider            | ✅    | ❌       |
| Stepper           | ✅    | ❌       |
| Switch            | ❌    | ❌       |
| TableView         | ❌    | ❌       |
| TimePicker        | ❌    | ✅       |
| WebView           | ❌    | ❌       |
| ContentPresenter  | ✅    | ✅       |
| ContentView       | ✅    | ✅       |
| Frame             | ✅    | ❌       |
| ScrollView        | ✅    | ❌       |
| TemplatedView     | ✅    | ✅       |
| AbsoluteLayout    | ✅    | ✅       |
| Grid              | ✅    | ✅       |
| RelativeLayout    | ✅    | ✅       |
| StackLayout       | ✅    | ✅       |

### Parameters

* On
    * Effect On/Off (true is On)
* Command
    * Tap Command
* CommandParameter
    * Tap Command Parameter
* LongCommand
    * Long Tap Command
* LongCommandParameter
    * Long Tap Command Parameter
* EffectColor
    * background color when to tap. if it doesn't setting,nothing will occur.
* EnableRipple
    * Ripple Effect On/Off (default true,android only)<br>
      If you don't have to use ripple effect, it make EnableRipple false.
* EnableSound
    * When tapped, whether play system sound effect.(Default false)
* SyncCanExecute
    * Whether synchronize Command's CanExecute to xamarin.forms.view's IsEnabled.(Default false)
    * If true, a view become opacity when disabled.

### How to write with Xaml

```xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
		xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
		xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
		x:Class="AiEffects.Sample.Views.AddCommandPage">

        <StackLayout>
    		<Label Text="Label"
    			ef:AddCommand.On="true"
    			ef:AddCommand.EffectColor="#50FFFF00"
    			ef:AddCommand.Command="{Binding EffectCommand}"
    			ef:AddCommand.CommandParameter="Label"
                ef:AddCommand.LongCommand="{Binding LongTapCommand}"
                ef:AddCommand.LongCommandParameter="LongTap" />
        </StackLayout>
</ContentPage>
```

### Limitation

**On Android**

* If the version is more than or equal 1.1.0 and RippleEffect is applied to a Element which is a kind of Layout, the InputTransparent of  the children become not to work.

### Tips

#### Changing Sound Effect

AppDelegate
```cs
public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
    global::Xamarin.Forms.Forms.Init();

    AiForms.Effects.iOS.Effects.Init();
    //here specify sound number
    AiForms.Effects.iOS.AddCommandPlatformEffect.PlaySoundNo = 1104;
    ...
}
```

MainActivity
```cs
protected override void OnCreate(Bundle bundle) {
    
    base.OnCreate(bundle);
    ...
    
    global::Xamarin.Forms.Forms.Init(this, bundle);
    
    //here specify SE
    AiForms.Effects.Droid.AddCommandPlatformEffect.PlaySoundEffect = Android.Media.SoundEffect.Spacebar;
    
    ...
}
```

#### When using Image

Ripple Effect will not occur foreground. In that case wrap by a layout view.

```xml
<StackLayout ef:AddCommand.On="{Binding EffectOn}"
			 ef:AddCommand.EffectColor="{Binding EffectColor}">
    <Image Source="image" />
</StackLayout>
```

## AddNumberPicker

This Effect add NumberPicker function to a view.<br>
When you tap the view ,Picker is shown. And when you select a number,it reflects to Number property.If you set Command property,it executes.

<img src="images/numberpicker1.gif" height=400 /> <img src="images/numberpicker2.gif" height=400 />

### Supported View

* Label
* BoxView
* Button
* Image
* StackLayout
* AbsoluteLayout

and more. same with AddCommand.

### Parameters

* On
    * Effect On/Off (true is On)
* Min
	* minimum number(positive integer)
* Max
	* maximum number(positive integer)
* Number
	* current number(default twoway binding)
* Title
	* Picker Title(optional)
	* In case iOS, if this is so long, it will be not beautiful.
* Command
    * command invoked when a number was picked(optional)


### How to write with Xaml

```xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
		xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
		xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
		xmlns:prism="clr-namespace:Prism.Mvvm;assembly=Prism.Forms"
		prism:ViewModelLocator.AutowireViewModel="True"
		x:Class="AiEffects.Sample.Views.AddNumberPickerPage"
		Title="AddNumberPicker">
	<StackLayout>
		<Label Text="Text"
			ef:AddNumberPicker.On="true"
			ef:AddNumberPicker.Min="10"
			ef:AddNumberPicker.Max="999"
			ef:AddNumberPicker.Number="{Binding Number}"
			ef:AddNumberPicker.Title="Select your number"
            ef:AddNumberPicker.Command="{Binding SomeCommand}" />
    </StackLayout>
</ContentPage>
```

## AddTimePicker

This is the effect that add TimePicker to a view.  
When you tap the view, Picker is shown. And when a time is selected, that time will be  reflected to Time property. If Command property is set, the command will be executed.

This effect supports views same with AddCommand.

### Parameters

* On
    * Effect On/Off (true is On)
* Time
	* current time(default twoway binding)
* Title
	* Picker Title(optional)
	* In case iOS, if this is so long, it will be not beautiful.
* Command
    * command invoked when a time was picked(optional)

## AddDatePicker

This is the effect that add DatePicker to a view.  
When you tap the view, Picker is shown. And when a date is selected, that date will be  reflected to Date property. If Command property is set, the command will be executed.

This effect supports views same with AddCommand.

### Parameters

* On
    * Effect On/Off (true is On)
* MinDate
	* minimum date(optional)
* MaxDate
	* maximum date(optional)
* Date
	* current date(default twoway binding)
* TodayText
	* button text to select today(optional / only iOS)
	* If this property is set, today's button will be shown. If that button is tapped, today will be selected.
* Command
    * command invoked when a date was picked(optional)

## AlterLineHeight

This Effect alter LineHeight of Label and Editor.

<img src="images/lineheight1.gif" width=250 /> <img src="images/lineheight2.jpg" width=250 />

### Supported View

* Label
* Editor

### Parameters

* On
    * Effect On/Off (true is On)
* Multiple
	* Multiple to the font height.
	* The font height * this multiple will become line height.

### How to write with Xaml

```xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
		xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
		xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Effects"
		xmlns:prism="clr-namespace:Prism.Mvvm;assembly=Prism.Forms"
		prism:ViewModelLocator.AutowireViewModel="True"
		x:Class="AiEffects.Sample.Views.AlterLineHeightPage"
		Title="AlterLineHeight">
	<StackLayout BackgroundColor="White" Spacing="4">
		<Label Text="{Binding LabelText}" VerticalOptions="Start" FontSize="12"
			ef:AlterLineHeight.On="true"
			ef:AlterLineHeight.Multiple="1.5"  />
	</StackLayout>
</ContentPage>
```

## AlterColor

This is the effect that alter the color of an element which it cannot change color normally.  

<img src="images/altercolor_ios.gif" /> <img src="images/altercolor_droid.gif" />

### Supported views

|                   | iOS  | Android | which part |
| ----------------- | ---- | ------- | ---------- |
| Page              |      | ✅       | Statusbar  |
| Slider            | ✅    | ✅       | Trackbar   |
| Switch            | ✅    | ✅       | Trackbar   |
| Entry             |      | ✅       | Under line |
| Editor            |      | ✅       | Under line |

### Parameters

* On
    * Effect On/Off (true is On)
* Accent
	* changed color.

### How to write with Xaml

```xml
<Slider Minimum="0" Maximum="1" Value="0.5" 
	ef:AlterColor.On="true" ef:AlterColor.Accent="Red" />
```


## Contributors

* [yuka-abn](https://github.com/yuka-abn)

## License

MIT Licensed.
