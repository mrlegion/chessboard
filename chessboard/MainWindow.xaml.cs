using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace chessboard
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int[][] _horseMoves = new[]
        {
            new[] {-2, 1}, new[] {-2, -1},
            new[] {-1, 2}, new[] {1, 2},
            new[] {2, -1}, new[] {2, 1},
            new[] {1, -2}, new[] {-1, -2},
        };

        private int _step = 1;
        private int _currentX;
        private int _currentY;

        private int _currentCell;

        private List<CellView> _cells;

        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoardCell();
        }

        private void DrawBoardCell()
        {
            int x = 1;
            int y = 8;

            for (int i = 0; i < 64; i++)
            {
                if (x > 8)
                {
                    x = 1;
                    y--;
                }

                var cell = new CellView();
                cell.SetPosition(x++, y);
                if ((x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0))
                    cell.Color = Brushes.BurlyWood;
                else cell.Color = Brushes.SaddleBrown;

                BoardGrid.Children.Add(cell);
            }

            _cells = BoardGrid.Children.OfType<CellView>().ToList();
        }

        private void GenerateNewPosition()
        {
            _currentCell = new Random().Next(0, 64);
            var firstCell = (CellView)BoardGrid.Children[_currentCell];
            firstCell.IsHorse = true;
            firstCell.IsSet = true;
            firstCell.Text = "H";
        }

        private void ClearAll()
        {
            _step = 1;
            BoardGrid.Children.Clear();
            DrawBoardCell();
        }

        private void Calculated()
        {

            _currentX = _cells[_currentCell].X;
            _currentY = _cells[_currentCell].Y;

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(MoveHandler);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            _timer.Start();
        }

        private void MoveHandler(object sender, EventArgs e)
        {
            Move();
        }

        private void Move()
        {
            if (_step == 63) { _timer.Stop(); StartBtn.IsEnabled = true; }
            
            var possibleMoves = GetPossibleMoves(_horseMoves, _currentX, _currentY);

            var nextPossibleMovesCount = new List<int>();
            foreach (CellView cellView in possibleMoves)
            {
                int x = cellView.X;
                int y = cellView.Y;
                nextPossibleMovesCount.Add(GetPossibleMoves(_horseMoves, x, y).Count);
            }

            int index = nextPossibleMovesCount.IndexOf(nextPossibleMovesCount.Min());
            _cells[_currentCell].IsHorse = false;
            _cells[_currentCell].Text = (_step++).ToString();
            StepTextLog.Text = _step.ToString();
            // set new cell
            CellView nextCell = possibleMoves[index];
            nextCell.IsHorse = true;
            nextCell.IsSet = true;
            nextCell.Text = "H";

            _currentX = nextCell.X;
            _currentY = nextCell.Y;

            _currentCell = _cells.IndexOf(nextCell);
        }

        private List<CellView> GetPossibleMoves(int[][] horseMovies, int currentX, int currentY)
        {
            List<CellView> cells = new List<CellView>();
            foreach (int[] move in horseMovies)
            {
                int x = currentX + move[0];
                int y = currentY + move[1];
                if (x < 9 && x >= 1)
                    if (y < 9 && y >= 1)
                    {
                        var request = _cells.FirstOrDefault(cv => cv.X == x && cv.Y == y);
                        if (request != null && !request.IsSet)
                            cells.Add(request);
                    }
            }

            return cells;
        }

        private void StartCalculated(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;
            ClearAll();
            GenerateNewPosition();
            Calculated();
        }
    }
}
