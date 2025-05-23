using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ConsoleApp1.Games;
using ConsoleApp1.Saving;

namespace ChessGames.UI
{
    public class GameBoardForm : Form
    {
        private const int TileSize = 60;
        private const int GridSize = 8;
        private Panel[,] tiles = new Panel[GridSize, GridSize];
        private object gameLogic;
        private Dictionary<string, Image> pieceImages;
        private Image whiteTileBackground;
        private Image blackTileBackground;
        private (int row, int col)? selectedTile = null;
        private GameSaving gameSaving = new GameSaving();

        // HUD
        private Label lblWhiteName, lblBlackName;
        private Label lblWhiteTimer, lblBlackTimer;
        private FlowLayoutPanel pnlWhiteCaptured, pnlBlackCaptured;
        private Timer timer;
        private int whiteTime, blackTime; // seconds left
        private bool isWhiteTurn;
        private string whitePlayerName, blackPlayerName;
        private List<string> whiteCaptured = new List<string>();
        private List<string> blackCaptured = new List<string>();

        public GameBoardForm(object logic, string whiteName, string blackName, int timeInMinutes)
        {
            gameLogic = logic;
            whitePlayerName = whiteName;
            blackPlayerName = blackName;
            whiteTime = blackTime = timeInMinutes * 60;
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeComponent()
        {
            int hudWidth = 100;
            int boardWidth = TileSize * GridSize;
            int padding = 20;
            int totalWidth = hudWidth * 2 + boardWidth + padding * 2;

            this.Text = "ChessGames - Game Board";
            this.Size = new Size(totalWidth + 40, TileSize * GridSize + 80);
            this.BackColor = Color.Black;

            // Left HUD (White player)
            lblWhiteName = new Label
            {
                Text = whitePlayerName,
                ForeColor = Color.White,
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                Location = new Point(padding, 60),
                AutoSize = true
            };
            lblWhiteTimer = new Label
            {
                Text = FormatTime(whiteTime),
                ForeColor = Color.LightGreen,
                Font = new Font("Comic Sans MS", 14, FontStyle.Bold),
                Location = new Point(padding, 100),
                AutoSize = true
            };
            pnlWhiteCaptured = new FlowLayoutPanel
            {
                Location = new Point(padding, 150),
                Size = new Size(hudWidth - 20, TileSize * GridSize - 100),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown
            };

            // Right HUD (Black player)
            int rightHudX = padding + hudWidth + boardWidth + padding;
            lblBlackName = new Label
            {
                Text = blackPlayerName,
                ForeColor = Color.White,
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                Location = new Point(rightHudX, 60),
                AutoSize = true
            };
            lblBlackTimer = new Label
            {
                Text = FormatTime(blackTime),
                ForeColor = Color.LightGreen,
                Font = new Font("Comic Sans MS", 14, FontStyle.Bold),
                Location = new Point(rightHudX, 100),
                AutoSize = true
            };
            pnlBlackCaptured = new FlowLayoutPanel
            {
                Location = new Point(rightHudX, 150),
                Size = new Size(hudWidth - 20, TileSize * GridSize - 100),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown
            };

            // Add controls to form
            this.Controls.Add(lblWhiteName);
            this.Controls.Add(lblWhiteTimer);
            this.Controls.Add(pnlWhiteCaptured);
            this.Controls.Add(lblBlackName);
            this.Controls.Add(lblBlackTimer);
            this.Controls.Add(pnlBlackCaptured);

            // Timer setup
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        private void InitializeGame()
        {
            pieceImages = new Dictionary<string, Image>();
            LoadTileBackgrounds();
            LoadPieceImages();
            CreateGameBoard();
            RenderPieces();
            whiteCaptured.Clear();
            blackCaptured.Clear();
            UpdateCapturedPanels();
            isWhiteTurn = true;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isWhiteTurn)
            {
                whiteTime--;
                lblWhiteTimer.Text = FormatTime(whiteTime);
                if (whiteTime <= 0)
                {
                    timer.Stop();
                    MessageBox.Show($"{whitePlayerName} ran out of time! {blackPlayerName} wins!");
                }
            }
            else
            {
                blackTime--;
                lblBlackTimer.Text = FormatTime(blackTime);
                if (blackTime <= 0)
                {
                    timer.Stop();
                    MessageBox.Show($"{blackPlayerName} ran out of time! {whitePlayerName} wins!");
                }
            }
        }

        private string FormatTime(int seconds)
        {
            return $"{seconds / 60:D2}:{seconds % 60:D2}";
        }

        private void UpdateCapturedPanels()
        {
            pnlWhiteCaptured.Controls.Clear();
            foreach (var piece in whiteCaptured)
                if (pieceImages.ContainsKey(piece))
                    pnlWhiteCaptured.Controls.Add(new PictureBox { Image = pieceImages[piece], SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(28, 28), Margin = new Padding(2), Enabled = false });
            pnlBlackCaptured.Controls.Clear();
            foreach (var piece in blackCaptured)
                if (pieceImages.ContainsKey(piece))
                    pnlBlackCaptured.Controls.Add(new PictureBox { Image = pieceImages[piece], SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(28, 28), Margin = new Padding(2), Enabled = false });
        }

        // Load images for tile backgrounds and pieces
        private void LoadTileBackgrounds()
        {
            string imagePath = @"..\\..\\..\\UI\\Image\\";
            try
            {
                whiteTileBackground = Image.FromFile(Path.Combine(imagePath, "white.png"));
                blackTileBackground = Image.FromFile(Path.Combine(imagePath, "black.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tile backgrounds: {ex.Message}"); // Handle error
            }
        }

        private void LoadPieceImages()
        {
            string imagePath = @"..\\..\\..\\UI\\Image\\";
            try
            {
                if (gameLogic is ChessLogic)
                {
                    imagePath = Path.Combine(imagePath, "Chess\\");
                    pieceImages["WPawn"] = Image.FromFile(Path.Combine(imagePath, "w_pawn.png"));
                    pieceImages["BPawn"] = Image.FromFile(Path.Combine(imagePath, "b_pawn.png"));
                    pieceImages["WRook"] = Image.FromFile(Path.Combine(imagePath, "w_rook.png"));
                    pieceImages["BRook"] = Image.FromFile(Path.Combine(imagePath, "b_rook.png"));
                    pieceImages["WKnight"] = Image.FromFile(Path.Combine(imagePath, "w_knight.png"));
                    pieceImages["BKnight"] = Image.FromFile(Path.Combine(imagePath, "b_knight.png"));
                    pieceImages["WBishop"] = Image.FromFile(Path.Combine(imagePath, "w_bishop.png"));
                    pieceImages["BBishop"] = Image.FromFile(Path.Combine(imagePath, "b_bishop.png"));
                    pieceImages["WQueen"] = Image.FromFile(Path.Combine(imagePath, "w_queen.png"));
                    pieceImages["BQueen"] = Image.FromFile(Path.Combine(imagePath, "b_queen.png"));
                    pieceImages["WKing"] = Image.FromFile(Path.Combine(imagePath, "w_king.png"));
                    pieceImages["BKing"] = Image.FromFile(Path.Combine(imagePath, "b_king.png"));
                }
                else if (gameLogic is CheckersLogic)
                {
                    imagePath = Path.Combine(imagePath, "Checkers\\");
                    pieceImages["W"] = Image.FromFile(Path.Combine(imagePath, "w_puck.png"));
                    pieceImages["B"] = Image.FromFile(Path.Combine(imagePath, "b_puck.png"));
                    pieceImages["WK"] = Image.FromFile(Path.Combine(imagePath, "w_puck_king.png"));
                    pieceImages["BK"] = Image.FromFile(Path.Combine(imagePath, "b_puck_king.png"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading piece images: {ex.Message}"); // Handle error
            }
        }


        private void CreateGameBoard()
        {
            int hudWidth = 100;
            int padding = 20;
            int boardX = padding + hudWidth;
            int boardY = 20;

            Panel boardPanel = new Panel
            {
                Location = new Point(boardX, boardY),
                Size = new Size(TileSize * GridSize, TileSize * GridSize),
                BackColor = Color.Black
            };
            this.Controls.Add(boardPanel);

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Panel tile = new Panel
                    {
                        Size = new Size(TileSize, TileSize),
                        Location = new Point(col * TileSize, row * TileSize),
                        BackgroundImage = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground,
                        BackgroundImageLayout = ImageLayout.Stretch
                    };

                    int capturedRow = row;
                    int capturedCol = col;
                    tile.Click += (sender, e) => Tile_Click(capturedRow, capturedCol);

                    tiles[row, col] = tile;
                    boardPanel.Controls.Add(tile);
                }
            }
        }

        private void Tile_Click(int row, int col)
        {
            if (selectedTile == null)
            {
                string piece = GetPieceAt(row, col);
                if (piece != null)
                {
                    if ((gameLogic is ChessLogic chessLogic &&
                        ((chessLogic.Board[row, col].StartsWith("W") && chessLogicIsWhiteTurn(chessLogic)) ||
                         (chessLogic.Board[row, col].StartsWith("B") && !chessLogicIsWhiteTurn(chessLogic))))
                        ||
                        (gameLogic is CheckersLogic checkersLogic &&
                        ((checkersLogic.Board[row, col].StartsWith("W") && checkersLogicIsWhiteTurn(checkersLogic)) ||
                         (checkersLogic.Board[row, col].StartsWith("B") && !checkersLogicIsWhiteTurn(checkersLogic))))
                    )
                    {
                        selectedTile = (row, col);
                        HighlightTile(row, col, Color.Red);
                    }
                }
            }
            else
            {
                var (startRow, startCol) = selectedTile.Value;
                bool moved = false;
                string capturedPiece = null;

                if (gameLogic is ChessLogic chessLogic)
                {
                    capturedPiece = chessLogic.Board[row, col];
                    moved = chessLogic.MovePiece(startRow, startCol, row, col);
                }
                else if (gameLogic is CheckersLogic checkersLogic)
                {
                    capturedPiece = checkersLogic.Board[row, col];
                    moved = checkersLogic.MovePiece(startRow, startCol, row, col);
                }

                if (moved)
                {
                    // For Chess, check for promotion
                    if (gameLogic is ChessLogic)
                        HandlePawnPromotion(row, col);

                    RenderPieces();

                    // Chess win
                    if (gameLogic is ChessLogic chessLogic2 && chessLogic2.IsCheckmate())
                    {
                        timer.Stop();
                        string winner = isWhiteTurn ? blackPlayerName : whitePlayerName;
                        MessageBox.Show($"Checkmate! {winner} wins!");

                        // Record win in JSON
                        var playerSaving = new PlayerSaving();
                        playerSaving.RecordWin(winner, "chess");

                        try
                        {
                            string savePath = "../../../Saving/Saves/game.json";
                            if (System.IO.File.Exists(savePath))
                                System.IO.File.Delete(savePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Could not delete game save: " + ex.Message);
                        }

                        this.Close();
                        return;
                    }

                    // Checkers win
                    if (gameLogic is CheckersLogic checkersLogic2)
                    {
                        if (checkersLogic2.IsWin(out string winnerColor))
                        {
                            timer.Stop();
                            string winnerName = winnerColor == "W" ? whitePlayerName : blackPlayerName;
                            MessageBox.Show($"{winnerName} wins!");

                            // Record win in JSON
                            var playerSaving = new PlayerSaving();
                            playerSaving.RecordWin(winnerName, "checkers");

                            try
                            {
                                string savePath = "../../../Saving/Saves/game.json";
                                if (System.IO.File.Exists(savePath))
                                    System.IO.File.Delete(savePath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Could not delete game save: " + ex.Message);
                            }

                            this.Close();
                            return;
                        }
                    }

                    isWhiteTurn = !isWhiteTurn;

                    // Save game after every move 
                    SaveCurrentGame();
                }
                else
                {
                    MessageBox.Show("Invalid move!");
                }

                UnhighlightTile(startRow, startCol);
                selectedTile = null;
            }
        }

        private void SaveCurrentGame()
        {
            List<List<string>> boardList = null;
            if (gameLogic is ChessLogic chessLogic)
                boardList = ToListBoard(chessLogic.Board);
            else if (gameLogic is CheckersLogic checkersLogic)
                boardList = ToListBoard(checkersLogic.Board);

            var gameState = new GameState
            {
                Board = boardList,
                WhiteCaptured = whiteCaptured,
                BlackCaptured = blackCaptured,
                WhiteTime = whiteTime,
                BlackTime = blackTime,
                IsWhiteTurn = isWhiteTurn,
                WhitePlayerName = whitePlayerName,
                BlackPlayerName = blackPlayerName,
                GameType = (gameLogic is ChessLogic) ? "Chess" : "Checkers"
            };
            gameSaving.SaveGame(gameState);
        }

        // Helper methods to access turn state
        private bool chessLogicIsWhiteTurn(ChessLogic logic)
        {
            var field = typeof(ChessLogic).GetField("isWhiteTurn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (bool)field.GetValue(logic);
        }
        private bool checkersLogicIsWhiteTurn(CheckersLogic logic)
        {
            var field = typeof(CheckersLogic).GetField("isWhiteTurn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (bool)field.GetValue(logic);
        }

        private string GetPieceAt(int row, int col)
        {
            if (gameLogic is ChessLogic chessLogic)
                return chessLogic.Board[row, col];
            if (gameLogic is CheckersLogic checkersLogic)
                return checkersLogic.Board[row, col];
            return null;
        }

        private void HighlightTile(int row, int col, Color color)
        {
            tiles[row, col].BorderStyle = BorderStyle.Fixed3D;
            tiles[row, col].BackColor = color;
        }

        private void UnhighlightTile(int row, int col)
        {
            tiles[row, col].BorderStyle = BorderStyle.None;
            tiles[row, col].BackColor = Color.Empty;
            tiles[row, col].BackgroundImage = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground;
        }

        private void RenderPieces()
        {
            string[,] board = null;

            if (gameLogic is ChessLogic chessLogic)
            {
                board = chessLogic.Board;
            }
            else if (gameLogic is CheckersLogic checkersLogic)
            {
                board = checkersLogic.Board;
            }

            if (board == null) return;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    string piece = board[row, col];

                    tiles[row, col].BackgroundImage = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground;
                    tiles[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    tiles[row, col].Controls.Clear();

                    if (piece != null && pieceImages.ContainsKey(piece))
                    {
                        PictureBox piecePictureBox = new PictureBox
                        {
                            Image = pieceImages[piece],
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Dock = DockStyle.Fill,
                            BackColor = Color.Transparent,
                            Enabled = false
                        };

                        tiles[row, col].Controls.Add(piecePictureBox);
                    }
                }
            }
        }

        private void HandlePawnPromotion(int row, int col)
        {
            if (gameLogic is ChessLogic chessLogic)
            {
                string piece = chessLogic.Board[row, col];
                if ((piece == "WPawn" && row == 0) || (piece == "BPawn" && row == 7))
                {
                    bool isWhite = piece.StartsWith("W");
                    using (var dialog = new PromotionDialog(isWhite))
                    {
                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            chessLogic.Board[row, col] = dialog.SelectedPiece;
                            RenderPieces();
                        }
                    }
                }
            }
        }

        private List<List<string>> ToListBoard(string[,] board)
        {
            var list = new List<List<string>>();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                var row = new List<string>();
                for (int j = 0; j < board.GetLength(1); j++)
                    row.Add(board[i, j]);
                list.Add(row);
            }
            return list;
        }
    }

    public class PromotionDialog : Form
    {
        public string SelectedPiece { get; private set; }

        // Pawn promotion dialog
        public PromotionDialog(bool isWhite)
        {
            this.Text = "Pawn Promotion";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Width = 340;
            this.Height = 120;

            var pieces = new[] { "Queen", "Rook", "Bishop", "Knight" };
            for (int i = 0; i < pieces.Length; i++)
            {
                var btn = new Button
                {
                    Text = pieces[i],
                    Tag = pieces[i],
                    Width = 75,
                    Height = 40,
                    Left = 10 + i * 80,
                    Top = 20
                };
                btn.Click += (s, e) =>
                {
                    SelectedPiece = (isWhite ? "W" : "B") + (string)btn.Tag;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                this.Controls.Add(btn);
            }
        }
    }
}