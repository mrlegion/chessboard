using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace chessboard
{
    /// <summary>
    /// Логика взаимодействия для CellView.xaml
    /// </summary>
    public sealed partial class CellView : UserControl
    {
        private readonly Border _cell;
        private TextBlock _innerText;
        private Brush _color;

        public CellView()
        {
            InitializeComponent();

            _cell = new Border() { Name = "Cell" };

            _innerText = new TextBlock()
            {
                FontSize = 50d,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Name = "Number"
            };

            _cell.Child = _innerText;
            this.AddChild(_cell);

        }

        public static readonly DependencyProperty IsSetProperty = 
            DependencyProperty.Register(nameof(IsSet), typeof(bool), typeof(CellView),
                new PropertyMetadata(false, delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    if (o is CellView cv)
                        if (args.NewValue is bool value) return;
                            
                }));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Brush), typeof(CellView),
                new PropertyMetadata(Brushes.Wheat, delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    if (o is CellView cv)
                        if (args.NewValue is Brush color)
                        {
                            cv._cell.Background = color;
                            cv._color = cv._color ?? color;
                        }
                }));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CellView),
                new PropertyMetadata(default, delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    if (o is CellView cv)
                        if (args.NewValue is string text)
                            cv._innerText.Text = text;
                }));

        public static readonly DependencyProperty IsHorseProperty =
            DependencyProperty.Register(nameof(IsHorse), typeof(bool), typeof(CellView),
                new PropertyMetadata(false, delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    if (o is CellView cv)
                        if (args.NewValue is bool value)
                            cv.Color = value ? Brushes.GreenYellow : cv._color;
                }));

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(int), typeof(CellView),
                new PropertyMetadata(0, delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    if (o is CellView cv)
                        if (args.NewValue is bool value) return;
                }));

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(int), typeof(CellView),
                new PropertyMetadata(0, delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    if (o is CellView cv)
                        if (args.NewValue is bool value) return;
                }));

        public bool IsSet
        {
            get => (bool) GetValue(IsSetProperty);
            set => SetValue(IsSetProperty, value);
        }

        public Brush Color
        {
            get => (Brush) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsHorse {
            get => (bool) GetValue(IsHorseProperty);
            set => SetValue(IsHorseProperty, value);
        }

        public int X {
            get => (int)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }
        public int Y {
            get => (int)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
