using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Globalization;

namespace ScratchApp.Controls.Form;


[TemplatePart(FORMINPUT_label, typeof(Label))]
[TemplatePart(FORMINPUT_displayControl, typeof(ContentPresenter))]
[TemplatePart(FORMINPUT_editControl, typeof(ContentPresenter))]
[TemplatePart(FORMINPUT_displayBorder, typeof(Border))]
public class FormInput : TemplatedControl
{
    private const string FORMINPUT_label = "PART_Label";
    private Label _mainLabel;

    private const string FORMINPUT_displayControl = "PART_displayControl";
    private ContentPresenter _displayControl;

    private const string FORMINPUT_editControl = "PART_editControl";
    private ContentPresenter _editControl;

    private const string FORMINPUT_displayBorder = "PART_displayBorder";
    private Border _displayBorder;

    public bool IsReadOnly => EditingTemplate is null;
    private bool IsEditing { get; set; }
    private bool EditorHasErrors => DataValidationErrors.GetHasErrors((Control)_editControl.Content);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (!IsReadOnly && IsEditing && (e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Escape))
        {
            SetEditingState(false);
            if (e.Key == Key.Enter) // Enter as tab
            {
                var current = FocusManager.Instance?.Current;
                if (current != null)
                {
                    var next = KeyboardNavigationHandler.GetNext(current, NavigationDirection.Next);
                    if (next != null)
                    {
                        var editableEditor = Avalonia.VisualTree.VisualExtensions.FindDescendantOfType<TextBox>((Control)next);
                        FocusManager.Instance?.Focus(editableEditor, NavigationMethod.Directional);
                    }
                }
            }

            if (e.Key == Key.Escape)
            {
                var current = FocusManager.Instance?.Current;
                if (current != null)
                {
                    var next = KeyboardNavigationHandler.GetNext(Parent, NavigationDirection.Previous);
                    if (next != null)
                    {
                        FocusManager.Instance?.Focus(next, NavigationMethod.Directional);
                    }
                }
            }
        }
    }

    private void EditControl_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsReadOnly && IsEditing)
        {
            SetEditingState(false);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!IsReadOnly)
        {
            SetEditingState(true);
        }
    }

    private void EditControl_GotFocus(object sender, GotFocusEventArgs e)
    {
        if (!IsReadOnly)
        {
            SetEditingState(true);
        }
    }

    private void MainLabel_DoubleTapped(object sender, TappedEventArgs e)
    {
        SetEditingState(true);
        try
        {
            var canSelectAllMethod = _editControl.Content.GetType().GetMethod("SelectAll");
            canSelectAllMethod?.Invoke(_editControl.Content, null);
        }
        catch { }
    }

    private void SetEditingState(bool isEditing)
    {
        if (!EditorHasErrors)
        {
            _displayControl.Opacity = isEditing ? 0.0 : 1.0;
            _displayBorder.Opacity = isEditing ? 0.0 : 1.0;
            _editControl.Opacity = isEditing ? 1.0 : 0.0;
            IsEditing = isEditing;
            if (isEditing)
            {
                ((Control)_editControl.Content).LostFocus += EditControl_LostFocus;
                ((Control)_editControl.Content).Focus();
            }
            else
            {
                ((Control)_editControl.Content).LostFocus -= EditControl_LostFocus;
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _mainLabel = e.NameScope.Find<Label>(FORMINPUT_label);
        _mainLabel.DoubleTapped += MainLabel_DoubleTapped;
        _displayControl = e.NameScope.Find<ContentPresenter>(FORMINPUT_displayControl);
        _displayBorder = e.NameScope.Find<Border>(FORMINPUT_displayBorder);
        _editControl = e.NameScope.Find<ContentPresenter>(FORMINPUT_editControl);
        _editControl.Opacity = 0.0;

        _mainLabel.SetValue(HorizontalAlignmentProperty, Avalonia.Layout.HorizontalAlignment.Left);
        _mainLabel.SetValue(VerticalAlignmentProperty, Avalonia.Layout.VerticalAlignment.Center);
        _mainLabel.SetValue(MarginProperty, new Thickness(0, 0, 3, 0));
        _mainLabel.SetValue(Grid.ColumnProperty, 0);
        _mainLabel.SetValue(Grid.RowProperty, 1);
        if (LabelOrientation.HasValue)
        {
            switch (LabelOrientation)
            {
                case Form.LabelOrientation.Left:
                    _mainLabel.SetValue(HorizontalAlignmentProperty, Avalonia.Layout.HorizontalAlignment.Left);
                    _mainLabel.SetValue(VerticalAlignmentProperty, Avalonia.Layout.VerticalAlignment.Center);
                    _mainLabel.SetValue(MarginProperty, new Thickness(0, 0, 3, 0));
                    _mainLabel.SetValue(Grid.ColumnProperty, 0);
                    _mainLabel.SetValue(Grid.RowProperty, 1);
                    break;
                case Form.LabelOrientation.Right:
                    _mainLabel.SetValue(HorizontalAlignmentProperty, Avalonia.Layout.HorizontalAlignment.Left);
                    _mainLabel.SetValue(VerticalAlignmentProperty, Avalonia.Layout.VerticalAlignment.Center);
                    _mainLabel.SetValue(MarginProperty, new Thickness(3, 0, 0, 0));
                    _mainLabel.SetValue(Grid.ColumnProperty, 2);
                    _mainLabel.SetValue(Grid.RowProperty, 1);
                    break;
                case Form.LabelOrientation.Above:
                    _mainLabel.SetValue(HorizontalAlignmentProperty, Avalonia.Layout.HorizontalAlignment.Left);
                    _mainLabel.SetValue(VerticalAlignmentProperty, Avalonia.Layout.VerticalAlignment.Bottom);
                    _mainLabel.SetValue(MarginProperty, new Thickness(0, 0, 0, 3));
                    _mainLabel.SetValue(Grid.ColumnProperty, 1);
                    _mainLabel.SetValue(Grid.RowProperty, 0);
                    break;
                case Form.LabelOrientation.Below:
                    _mainLabel.SetValue(HorizontalAlignmentProperty, Avalonia.Layout.HorizontalAlignment.Left);
                    _mainLabel.SetValue(VerticalAlignmentProperty, Avalonia.Layout.VerticalAlignment.Top);
                    _mainLabel.SetValue(MarginProperty, new Thickness(0, 3, 0, 0));
                    _mainLabel.SetValue(Grid.ColumnProperty, 1);
                    _mainLabel.SetValue(Grid.RowProperty, 2);
                    break;
            }
        }

        var displayElement = GenerateDisplayElement(DataContext);
        _displayControl.Content = displayElement;

        if (!IsReadOnly)
        {
            var editingElement = GenerateEditingElement(DataContext);
            _editControl.Content = editingElement;
            GotFocus += EditControl_GotFocus;
        }

        if (string.IsNullOrWhiteSpace(LabelText))
            _mainLabel.IsVisible = false;
    }

    public static readonly StyledProperty<string> LabelTextProperty =
    AvaloniaProperty.Register<FormInput, string>(
        name: nameof(LabelText),
        defaultValue: nameof(LabelText));

    public string LabelText
    {
        get { return GetValue(LabelTextProperty); }
        set { SetValue(LabelTextProperty, value); }
    }

    public static readonly StyledProperty<LabelOrientation?> LabelOrientationProperty =
    AvaloniaProperty.Register<FormInput, LabelOrientation?>(
        name: nameof(LabelOrientation),
        defaultValue: null);

    public LabelOrientation? LabelOrientation
    {
        get { return GetValue(LabelOrientationProperty); }
        set { SetValue(LabelOrientationProperty, value); }
    }

    private IDataTemplate _displayTemplate;
    public static readonly DirectProperty<FormInput, IDataTemplate> DisplayTemplateProperty =
        AvaloniaProperty.RegisterDirect<FormInput, IDataTemplate>(
            nameof(DisplayTemplate),
            o => o.DisplayTemplate,
            (o, v) => o.DisplayTemplate = v);

    public IDataTemplate DisplayTemplate
    {
        get { return _displayTemplate; }
        set { SetAndRaise(DisplayTemplateProperty, ref _displayTemplate, value); }
    }

    private IDataTemplate _editingTemplate;
    public static readonly DirectProperty<FormInput, IDataTemplate> EditingTemplateProperty =
            AvaloniaProperty.RegisterDirect<FormInput, IDataTemplate>(
                nameof(EditingTemplate),
                o => o.EditingTemplate,
                (o, v) => o.EditingTemplate = v);

    public IDataTemplate EditingTemplate
    {
        get => _editingTemplate;
        set => SetAndRaise(EditingTemplateProperty, ref _editingTemplate, value);
    }

    protected Control GenerateDisplayElement(object dataItem)
    {
        if (DisplayTemplate != null)
        {
            var displayControl = DisplayTemplate.Build(dataItem);
            displayControl.IsHitTestVisible = false;
            return displayControl;
        }
        if (Design.IsDesignMode)
        {
            return null;
        }
        else
        {
            throw FormInputError.MissingTemplate("Display Template");
        }
    }

    protected Control GenerateEditingElement(object dataItem)
    {
        if (EditingTemplate != null)
        {
            var editingControl = EditingTemplate.Build(dataItem);
            return editingControl;
        }
        if (Design.IsDesignMode)
        {
            return null;
        }
        else
        {
            throw FormInputError.MissingTemplate("Editing Template");
        }
    }
}

internal static class FormInputError
{

    public static TypeInitializationException MissingTemplate(string type)
    {
        return new TypeInitializationException(Format("Missing template.  Cannot initialize {0}.", type), null);
    }

    private static string Format(string formatString, params object[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, formatString, args);
    }
}
