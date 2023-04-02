using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using System.Collections.Generic;

namespace ScratchApp.Controls.Form;

[TemplatePart(FORMINPUTITEMSCONTROL_itemsControl, typeof(ItemsControl))]
public class FormInputItemsControl : TemplatedControl
{
    private const string FORMINPUTITEMSCONTROL_itemsControl = "PART_itemsControl";
    private ItemsControl _itemsControl;

    public FormInputItemsControl()
    {
        Loaded += FormInputItemsControl_Loaded;
    }

    private void FormInputItemsControl_Loaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (FormInputsPanel != _defaultPanel)
        {
            var panel = Avalonia.VisualTree.VisualExtensions.FindDescendantOfType<Panel>(_itemsControl);
            panel.SetValue(KeyboardNavigation.TabNavigationProperty, KeyboardNavigationMode.Local);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsControl = e.NameScope.Find<ItemsControl>(FORMINPUTITEMSCONTROL_itemsControl);

        if (FormInputs != null)
        {
            foreach (var input in FormInputs)
            {
                if (LabelOrientation.HasValue && !input.LabelOrientation.HasValue)
                    input.LabelOrientation = LabelOrientation.Value;

                var borderThickness = GetValue(BorderThicknessProperty);
                if (borderThickness != default && input.BorderThickness == default)
                    input.BorderThickness = borderThickness;

                var borderBrush = GetValue(BorderBrushProperty);
                if (borderBrush != default && input.BorderBrush == default)
                    input.BorderBrush = borderBrush;

                var itemMargin = GetValue(ItemMarginProperty);
                if (itemMargin != default && input.Margin == default)
                    input.SetValue(MarginProperty, itemMargin);
            }
        }
    }

    private static readonly FuncTemplate<Panel> _defaultPanel = new(() => { var panel = new StackPanel(); panel.SetValue(KeyboardNavigation.TabNavigationProperty, KeyboardNavigationMode.Local); return panel; });

    public static readonly StyledProperty<ITemplate<Panel>> FormInputsPanelProperty =
        AvaloniaProperty.Register<FormInputItemsControl, ITemplate<Panel>>(nameof(FormInputsPanel), _defaultPanel);

    public ITemplate<Panel> FormInputsPanel
    {
        get => GetValue(FormInputsPanelProperty);
        set => SetValue(FormInputsPanelProperty, value);
    }

    public static readonly StyledProperty<IEnumerable<FormInput>> FormInputsProperty =
        AvaloniaProperty.Register<FormInputItemsControl, IEnumerable<FormInput>>(nameof(FormInputsPanel), new List<FormInput>());

    public IEnumerable<FormInput> FormInputs
    {
        get => GetValue(FormInputsProperty);
        set => SetValue(FormInputsProperty, value);
    }

    public static readonly StyledProperty<LabelOrientation?> LabelOrientationProperty =
    AvaloniaProperty.Register<FormInputItemsControl, LabelOrientation?>(
        name: nameof(LabelOrientation),
        defaultValue: null);

    public LabelOrientation? LabelOrientation
    {
        get { return GetValue(LabelOrientationProperty); }
        set { SetValue(LabelOrientationProperty, value); }
    }

    public static readonly StyledProperty<Thickness> ItemMarginProperty =
        AvaloniaProperty.Register<FormInputItemsControl, Thickness>(nameof(FormInputsPanel), default);

    public Thickness ItemMargin
    {
        get => GetValue(ItemMarginProperty);
        set => SetValue(ItemMarginProperty, value);
    }
}