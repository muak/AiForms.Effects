# AiForms.Effects

Xamarin.Forms Effects for Android / iOS only.

## AddNumberPicker

This Effect add NumberPicker function to a view.
When you tap the view ,Picker is shown.And when you select a number,it reflects to Number property.If you set Command property,it executes.

### Supported View

* Label
* BoxView
* Button
* Image
* StackLayout
* AbsoluteLayout

### How to Xaml

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


## AlterLineHeight

This Effect alter LineHeight of Label and Editor.

### Supported View

* Label
* Editor

### How to Xaml

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

## AddCommand

This Effect add Command function to a view.

### Supported View

* Label
* BoxView
* Button
* StackLayout
* AbsoluteLayout
* Image
* Editor

### How to Xaml

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
    			ef:AddCommand.CommandParameter="Label" />
        </StackLayout>
</ContentPage>
```

## Installation instructions

```bash
Install-Package AiForms.Effects -Pre
```

### for iOS project

AppDelegate.cs

```csharp
public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
    global::Xamarin.Forms.Forms.Init();

    AiForms.Effects.iOS.Effects.Init();  //need to write here

    LoadApplication(new App(new iOSInitializer()));

    return base.FinishedLaunching(app, options);
}
```
