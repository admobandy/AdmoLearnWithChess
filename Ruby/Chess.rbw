require 'tk'

$firstClickSet = 0
$secondClickSet = 0
$firstClickX = 0
$firstClickY = 0
$secondClickX = 0
$secondClickY = 0
$whitesTurn = true
$whiteCanCastleQueenSide = true
$blackCanCastleQueenSide = true
$whiteCanCastleKingSide = true
$blackCanCastleKingSide = true
$whiteCastleQueenSide = false
$blackCastleQueenSide = false
$whiteCastleKingSide = false
$blackCastleKingSide = false
$whiteIsInCheck = false
$blackIsInCheck = false
$assessingCheck = false

chess = TkRoot.new do
    title "Chess"
    minsize(520,520)
end

$tileBrown = TkPhotoImage.new(:file => 'brownTile.gif') 
$tileBlack = TkPhotoImage.new(:file => 'blackTile.gif')
$blackBishop = TkPhotoImage.new(:file => 'bishopb.gif')
$whiteBishop = TkPhotoImage.new(:file => 'bishopw.gif')
$blackPawn = TkPhotoImage.new(:file => 'pawnb.gif')
$whitePawn = TkPhotoImage.new(:file => 'pawnw.gif')
$blackKnight = TkPhotoImage.new(:file => 'knightb.gif')
$whiteKnight = TkPhotoImage.new(:file => 'knightw.gif')
$whiteKing = TkPhotoImage.new(:file => 'kingw.gif')
$blackKing = TkPhotoImage.new(:file => 'kingb.gif')
$whiteQueen = TkPhotoImage.new(:file => 'queenw.gif')
$blackQueen = TkPhotoImage.new(:file => 'queenb.gif')
$blackRook = TkPhotoImage.new(:file => 'rookb.gif')
$whiteRook = TkPhotoImage.new(:file => 'rookw.gif')

$board = [
[TkButton.new(chess) do command (proc {buttonAction00}) end,TkButton.new(chess) do command (proc {buttonAction01}) end,TkButton.new(chess) do command (proc {buttonAction02}) end,TkButton.new(chess) do command (proc {buttonAction03}) end,TkButton.new(chess) do command (proc {buttonAction04}) end,TkButton.new(chess) do command (proc {buttonAction05}) end,TkButton.new(chess) do command (proc {buttonAction06}) end,TkButton.new(chess) do command (proc {buttonAction07}) end],
[TkButton.new(chess) do command (proc {buttonAction10}) end,TkButton.new(chess) do command (proc {buttonAction11}) end,TkButton.new(chess) do command (proc {buttonAction12}) end,TkButton.new(chess) do command (proc {buttonAction13}) end,TkButton.new(chess) do command (proc {buttonAction14}) end,TkButton.new(chess) do command (proc {buttonAction15}) end,TkButton.new(chess) do command (proc {buttonAction16}) end,TkButton.new(chess) do command (proc {buttonAction17}) end],
[TkButton.new(chess) do command (proc {buttonAction20}) end,TkButton.new(chess) do command (proc {buttonAction21}) end,TkButton.new(chess) do command (proc {buttonAction22}) end,TkButton.new(chess) do command (proc {buttonAction23}) end,TkButton.new(chess) do command (proc {buttonAction24}) end,TkButton.new(chess) do command (proc {buttonAction25}) end,TkButton.new(chess) do command (proc {buttonAction26}) end,TkButton.new(chess) do command (proc {buttonAction27}) end],
[TkButton.new(chess) do command (proc {buttonAction30}) end,TkButton.new(chess) do command (proc {buttonAction31}) end,TkButton.new(chess) do command (proc {buttonAction32}) end,TkButton.new(chess) do command (proc {buttonAction33}) end,TkButton.new(chess) do command (proc {buttonAction34}) end,TkButton.new(chess) do command (proc {buttonAction35}) end,TkButton.new(chess) do command (proc {buttonAction36}) end,TkButton.new(chess) do command (proc {buttonAction37}) end],
[TkButton.new(chess) do command (proc {buttonAction40}) end,TkButton.new(chess) do command (proc {buttonAction41}) end,TkButton.new(chess) do command (proc {buttonAction42}) end,TkButton.new(chess) do command (proc {buttonAction43}) end,TkButton.new(chess) do command (proc {buttonAction44}) end,TkButton.new(chess) do command (proc {buttonAction45}) end,TkButton.new(chess) do command (proc {buttonAction46}) end,TkButton.new(chess) do command (proc {buttonAction47}) end],
[TkButton.new(chess) do command (proc {buttonAction50}) end,TkButton.new(chess) do command (proc {buttonAction51}) end,TkButton.new(chess) do command (proc {buttonAction52}) end,TkButton.new(chess) do command (proc {buttonAction53}) end,TkButton.new(chess) do command (proc {buttonAction54}) end,TkButton.new(chess) do command (proc {buttonAction55}) end,TkButton.new(chess) do command (proc {buttonAction56}) end,TkButton.new(chess) do command (proc {buttonAction57}) end],
[TkButton.new(chess) do command (proc {buttonAction60}) end,TkButton.new(chess) do command (proc {buttonAction61}) end,TkButton.new(chess) do command (proc {buttonAction62}) end,TkButton.new(chess) do command (proc {buttonAction63}) end,TkButton.new(chess) do command (proc {buttonAction64}) end,TkButton.new(chess) do command (proc {buttonAction65}) end,TkButton.new(chess) do command (proc {buttonAction66}) end,TkButton.new(chess) do command (proc {buttonAction67}) end],
[TkButton.new(chess) do command (proc {buttonAction70}) end,TkButton.new(chess) do command (proc {buttonAction71}) end,TkButton.new(chess) do command (proc {buttonAction72}) end,TkButton.new(chess) do command (proc {buttonAction73}) end,TkButton.new(chess) do command (proc {buttonAction74}) end,TkButton.new(chess) do command (proc {buttonAction75}) end,TkButton.new(chess) do command (proc {buttonAction76}) end,TkButton.new(chess) do command (proc {buttonAction77}) end]
]

xOffset = 1
yOffset = 1
isBrown = 1
rowCount = 0
i = 0
num = 8
while i < num	
	j = 0
	if (rowCount == 0 || rowCount == 2 || rowCount == 4 || rowCount == 6 ) then
		isBrown = 1
	else
		isBrown = 0
	end		
    while j < num   
		if isBrown == 1 then
			$board[i][j].image = $tileBrown
			isBrown = 0
		else		
			$board[i][j].image = $tileBlack
			isBrown = 1
		end							
        $board[i][j].place('height' => $tileBlack.height, 'width' => $tileBlack.width, 'x' => xOffset, 'y' => yOffset)		
        xOffset = xOffset + 65       
		j +=1
    end
	rowCount = rowCount + 1
    xOffset = 1
    yOffset = yOffset + 65
	i +=1
end

$board[0][0].image = $blackRook
$board[0][1].image = $blackKnight
$board[0][2].image = $blackBishop
$board[0][3].image = $blackQueen
$board[0][4].image = $blackKing
$board[0][5].image = $blackBishop
$board[0][6].image = $blackKnight
$board[0][7].image = $blackRook
$board[1][0].image = $blackPawn
$board[1][1].image = $blackPawn 
$board[1][2].image = $blackPawn
$board[1][3].image = $blackPawn
$board[1][4].image = $blackPawn 
$board[1][5].image = $blackPawn
$board[1][6].image = $blackPawn
$board[1][7].image = $blackPawn
$board[6][0].image = $whitePawn
$board[6][1].image = $whitePawn
$board[6][2].image = $whitePawn
$board[6][3].image = $whitePawn
$board[6][4].image = $whitePawn
$board[6][5].image = $whitePawn
$board[6][6].image = $whitePawn
$board[6][7].image = $whitePawn
$board[7][0].image = $whiteRook
$board[7][1].image = $whiteKnight
$board[7][2].image = $whiteBishop
$board[7][3].image = $whiteQueen
$board[7][4].image = $whiteKing
$board[7][5].image = $whiteBishop
$board[7][6].image = $whiteKnight
$board[7][7].image = $whiteRook

def buttonAction00
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 0
	end	
end
def buttonAction01
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 1
	end
end
def buttonAction02
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 2
	end
end
def buttonAction03
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 3
	end
end
def buttonAction04
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 4
	end
end
def buttonAction05
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 5
	end
end
def buttonAction06
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 6
	end
end
def buttonAction07
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 0
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 0
		$firstClickY = 7
	end
end
def buttonAction10
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 0
	end
end
def buttonAction11
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 1
	end
end
def buttonAction12
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 2
	end
end
def buttonAction13
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 3
	end
end
def buttonAction14
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 4
	end
end
def buttonAction15
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 5
	end
end
def buttonAction16
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 6
	end
end
def buttonAction17
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 1
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 1
		$firstClickY = 7
	end
end
def buttonAction20
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 0
	end
end
def buttonAction21
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 1
	end
end
def buttonAction22
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 2
	end
end
def buttonAction23
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 3
	end
end
def buttonAction24
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 4
	end
end
def buttonAction25
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 5
	end
end
def buttonAction26
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 6
	end
end
def buttonAction27
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 2
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 2
		$firstClickY = 7
	end
end
def buttonAction30
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 0
	end
end
def buttonAction31
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 1
	end
end
def buttonAction32
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 2
	end
end
def buttonAction33
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 3
	end
end
def buttonAction34
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 4
	end
end
def buttonAction35
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 5
	end
end
def buttonAction36
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 6
	end
end
def buttonAction37
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 3
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 3
		$firstClickY = 7
	end
end
def buttonAction40
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 0
	end
end
def buttonAction41
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 1
	end
end
def buttonAction42
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 2
	end
end
def buttonAction43
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 3
	end
end
def buttonAction44
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 4
	end
end
def buttonAction45
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 5
	end
end
def buttonAction46
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 6
	end
end
def buttonAction47
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 4
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 4
		$firstClickY = 7
	end
end
def buttonAction50
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 0
	end
end
def buttonAction51
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 1
	end
end
def buttonAction52
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 2
	end
end
def buttonAction53
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 3
	end
end
def buttonAction54
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 4
	end
end
def buttonAction55
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 5
	end
end
def buttonAction56
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 6
	end
end
def buttonAction57
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 5
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 5
		$firstClickY = 7
	end
end
def buttonAction60
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 0
	end
end
def buttonAction61
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 1
	end
end
def buttonAction62
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 2
	end
end
def buttonAction63
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 3
	end
end
def buttonAction64
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 4
	end
end
def buttonAction65
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 5
	end
end
def buttonAction66
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 6
	end
end
def buttonAction67
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 6
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 6
		$firstClickY = 7
	end
end
def buttonAction70
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 0
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 0
	end
end
def buttonAction71
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 1
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 1
	end
end
def buttonAction72
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 2
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 2
	end
end
def buttonAction73
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 3
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 3
	end
end
def buttonAction74
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 4
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 4
	end
end
def buttonAction75
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 5
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 5
	end
end
def buttonAction76
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 6
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 6
	end
end
def buttonAction77
	if $secondClickSet == 0 && $firstClickSet == 1 then
		$secondClickSet = 1
		$secondClickX = 7
		$secondClickY = 7
		validateMove
		return
	end
	if $firstClickSet == 0 then
		$firstClickSet = 1
		$firstClickX = 7
		$firstClickY = 7
	end
end

def validateMove
	puts("First Click x,y #{$firstClickX}, #{$firstClickY} #{$board[$firstClickX][$firstClickY].image.path}\n")
	puts("Second Click x,y #{$secondClickX}, #{$secondClickY} #{$board[$secondClickX][$secondClickY].image.path}\n")
	firstXBkp = $firstClickX
	firstYBkp = $firstClickY
	secondXBkp = $secondClickX
	secondYBkp = $secondClickY
	firstClickImage = $board[firstXBkp][firstYBkp].image
	secondClickImage = $board[secondXBkp][secondYBkp].image
	if moveLegal
		doMove
		if !$assessingCheck then
			if ($whitesTurn) then
				#if white in check undo move
				
				#if whiteInCheck then
				#	$board[firstXBkp][firstYBkp].image = firstClickImage 
				#	$board[secondXBkp][secondYBkp].image = secondClickImage
				#	return false
				#end
				
				$whitesTurn = false
			else
				#if black in check undo move
				#if blackInCheck then
				#	$board[firstXBkp][firstYBkp].image = firstClickImage
				#	$board[secondXBkp][secondYBkp].image = secondClickImage
				#	return false
				#end
				$whitesTurn = true
			end
		end
		$firstClickSet = 0
		$secondClickSet = 0	
		return true
	end
	$firstClickSet = 0
	$secondClickSet = 0	
	return true
end

def doMove	
	if $whiteCastleQueenSide then
		$board[7][0].image = $tileBlack		
		$board[7][2].image = $whiteKing
		$board[7][3].image = $whiteRook
		$board[7][4].image = $tileBlack
		$whiteCanCastleQueenSide = false
		$whiteCanCastleKingSide = false
		$whiteCastleQueenSide = false
		return
	end
	
	if $whiteCastleKingSide then
		$board[7][4].image = $tileBlack
		$board[7][5].image = $whiteRook
		$board[7][6].image = $whiteKing
		$board[7][7].image = $tileBrown
		$whiteCanCastleQueenSide = false
		$whiteCanCastleKingSide = false
		$whiteCastleKingSide = false
		return
	end
	
	if $blackCastleQueenSide then
		$board[0][0].image = $tileBrown
		$board[0][2].image = $blackKing
		$board[0][3].image = $blackRook
		$board[0][4].image = $tileBrown
		$blackCanCastleQueenSide = false
		$blackCanCastleKingSide = false
		$blackCastleQueenSide = false
		return
	end
	
	if $blackCastleKingSide then
		$board[0][4].image = $tileBrown
		$board[0][5].image = $blackRook
		$board[0][6].image = $blackKing
		$board[0][7].image = $tileBlack
		$blackCanCastleQueenSide = false
		$blackCanCastleKingSide = false
		$blackCastleKingSide = false
		return
	end


	if !$assessingCheck then
		$board[$secondClickX][$secondClickY].image = $board[$firstClickX][$firstClickY].image
		if ($firstClickX + $firstClickY)%2 == 1 then
			$board[$firstClickX][$firstClickY].image = $tileBlack
		else
			$board[$firstClickX][$firstClickY].image = $tileBrown
		end
	end
end

def moveLegal
	firstClickPiece = $board[$firstClickX][$firstClickY].image
	secondClickPiece = $board[$secondClickX][$secondClickY].image
	firstIsWhite = false
	secondIsWhite = false
	firstIsBlack = false
	secondIsBlack = false
	
	#stop friendly capture
	if (firstClickPiece == $whiteRook || firstClickPiece == $whitePawn || firstClickPiece == $whiteKnight || firstClickPiece == $whiteBishop || firstClickPiece == $whiteQueen || firstClickPiece == $whiteKing) then
		firstIsWhite = true
	end	
	if (secondClickPiece == $whiteRook || secondClickPiece == $whitePawn || secondClickPiece == $whiteKnight || secondClickPiece == $whiteBishop || secondClickPiece == $whiteQueen || secondClickPiece == $whiteKing) then
		secondIsWhite = true
	end	
	if (firstClickPiece == $blackRook || firstClickPiece == $blackPawn || firstClickPiece == $blackKnight || firstClickPiece == $blackBishop || firstClickPiece == $blackQueen || firstClickPiece == $blackKing) then
		firstIsBlack = true
	end	
	if (secondClickPiece == $blackRook || secondClickPiece == $blackPawn || secondClickPiece == $blackKnight || secondClickPiece == $blackBishop || secondClickPiece == $blackQueen || secondClickPiece == $blackKing) then
		secondIsBlack = true
	end
	if (firstIsWhite && secondIsWhite) || (firstIsBlack && secondIsBlack) then
		#are we castling?
		if (firstIsWhite && firstClickPiece == $whiteKing && secondClickPiece == $whiteRook && !$whiteIsInCheck) then
			if xCollision then
				return false
			end
			#check for queen side or king side
			if ($secondClickY == 0 && $whiteCanCastleQueenSide) then
				#queen side
				$whiteCastleQueenSide = true
				return true
			end
			
			if ($secondClickY == 7 && $whiteCanCastleKingSide) then
				#king side
				$whiteCastleKingSide = true
				return true
			end			
		end
		if (firstIsBlack && firstClickPiece == $blackKing && secondClickPiece == $blackRook && !$blackIsInCheck) then
			if xCollision then
				return false
			end
			#check for queen side or king side
			if ($secondClickY == 0 && $blackCanCastleQueenSide) then
				#queen side
				$blackCastleQueenSide = true
				return true
			end
			
			if ($secondClickY == 7 && $blackCanCastleKingSide) then
				#king side
				$blackCastleKingSide = true
				return true
			end			
		end
		return false
	end
	
	if (firstIsWhite && $whitesTurn == false) then
		return false
	end
	
	if (firstIsBlack && $whitesTurn == true) then
		return false
	end
	
	#stop tile move
	if (firstClickPiece == $tileBrown || firstClickPiece == $tileBlack) then
		return false
	end
	
	#kings
	if (firstClickPiece == $whiteKing) || (firstClickPiece == $blackKing) then
		return validateKingMove
	end
	#knights
	if (firstClickPiece == $whiteKnight) || (firstClickPiece == $blackKnight) then
		return validateKnightMove
	end
	#bishops
	if (firstClickPiece == $whiteBishop) || (firstClickPiece == $blackBishop) then
		return validateBishopMove
	end
	#rooks
	if (firstClickPiece == $whiteRook) || (firstClickPiece == $blackRook) then
		return validateRookMove
	end
	#queens
	if (firstClickPiece == $whiteQueen) || (firstClickPiece == $blackQueen) then
		return validateQueenMove
	end
	#pawns	
	if (firstClickPiece == $whitePawn) || (firstClickPiece == $blackPawn) then
		return validatePawnMove
	end
	
	return true
end

def validateKingMove
	sX = $firstClickX
	sY = $firstClickY
	dX = $secondClickX
	dY = $secondClickY
	xDiff = (sX - dX).abs
	yDiff = (sY - dY).abs
	#D = 1 only, don't check for collision
	if (xDiff > 1) || (yDiff > 1) then
		return false
	end	
	#check for moving into check
	
	if($board[sX][sY].image == $whiteKing) then
		$whiteCanCastleQueenSide = false		
		$whiteCanCastleKingSide = false	
	end
	
	if($board[sX][sY].image == $blackKing) then		
		$blackCanCastleQueenSide = false
		$blackCanCastleKingSide = false
	end
	
	return true
end

def validateKnightMove
	sX = $firstClickX
	sY = $firstClickY
	dX = $secondClickX
	dY = $secondClickY	
	xDiff = (sX - dX).abs
	yDiff = (sY - dY).abs
	#don't check for collision
	if (xDiff == 2) && (yDiff == 1) then
		return true
	end
	
	if (xDiff == 1) && (yDiff == 2) then
		return true
	end	
	return false
end

def validateBishopMove
	#needs correction
	sX = $firstClickX
	sY = $firstClickY
	dX = $secondClickX
	dY = $secondClickY	
	xDiff = (sX - dX).abs
	yDiff = (sY - dY).abs
	#invalidate non-diagonalMove
	if (xDiff != yDiff) then
		return false
	end
	
	if (xDiff == 0) || (yDiff == 0) then
		return false
	end
	
	if (xDiff > 7) || (yDiff > 7) then
		return false
	end
	
	if (xDiff/yDiff == 1) then
		if xyCollision then
			return false
		end
		return true
	end
		
	return false
end

def validateRookMove
	sX = $firstClickX
	sY = $firstClickY
	dX = $secondClickX
	dY = $secondClickY	
	xDiff = (sX - dX).abs
	yDiff = (sY - dY).abs
	#invalidate diagonalMove
	if (xDiff > 1) && (yDiff > 1) then
		return false
	end
	
	if (xDiff > 7) || (yDiff > 7) then
		return false
	end
	
	if (xDiff == 0) then
		if yCollision then
			return false
		end
		
		if($board[sX][sY].image == $whiteRook) && (sY == 0) then
			$whiteCanCastleQueenSide = false
		end
		if($board[sX][sY].image == $whiteRook) && (sY == 7) then
			$whiteCanCastleKingSide = false
		end
		if($board[sX][sY].image == $blackRook) && (sY == 0) then
			$blackCanCastleQueenSide = false
		end
		if($board[sX][sY].image == $blackRook) && (sY == 7) then
			$blackCanCastleKingSide = false
		end
		return true
	end
	
	if (yDiff == 0) then
		if xCollision then
			return false
		end
		
		if($board[sX][sY].image == $whiteRook) && (sY == 0) then
			$whiteCanCastleQueenSide = false
		end
		if($board[sX][sY].image == $whiteRook) && (sY == 7) then
			$whiteCanCastleKingSide = false
		end
		if($board[sX][sY].image == $blackRook) && (sY == 0) then
			$blackCanCastleQueenSide = false
		end
		if($board[sX][sY].image == $blackRook) && (sY == 7) then
			$blackCanCastleKingSide = false
		end
		return true
	end
	
	return false
end

def validateQueenMove
	sX = $firstClickX
	sY = $firstClickY
	dX = $secondClickX
	dY = $secondClickY	
	xDiff = (sX - dX).abs
	yDiff = (sY - dY).abs
	#checkPathCollision if distance is > 1
	if (xDiff == 0) then
		#check y collision
		if yCollision then
			return false
		end
		return true
	end	
	
	if (yDiff == 0) then
		#check x collision
		if xCollision then
			return false
		end
		return true
	end
	
	if (xDiff > 7) || (yDiff > 7) then
		return false
	end
	
	if ((xDiff > 0) && (yDiff > 0)) && (xDiff/yDiff == 1) then
		#check xy collision
		if (xDiff != yDiff) then
			return false
		end		
		if xyCollision then
			return false
		end
		return true
	end	
	
	return false
end

def validatePawnMove
	#check for first move
	#diagonal capture only
	#can move two squares on first move
	sX = $firstClickX
	sY = $firstClickY
	dX = $secondClickX
	dY = $secondClickY	
	xDiff = (dX - sX)
	yDiff = (dY - sY)
	if ($whitesTurn) then
		#are we moving the wrong way?
		if (xDiff > 1 || xDiff == 1) then
			return false
		end
		#is it first move?
		if (sX == 6) then
			#is a two square move being attempted?
			if (xDiff == -2) then
				#is it blocked?
				if xCollision then
					return false
				end
				if(($board[sX-2][sY].image == $tileBrown || $board[sX-2][sY].image == $tileBlack) && yDiff == 0) then
					return true
				end
			end
			if ((xDiff == -1 && yDiff == 0) && ($board[sX-1][sY].image == $tileBrown || $board[sX-1][sY].image == $tileBlack)) then
				return true
			end
		end		
		#are we capturing?
		if ((xDiff == -1 && yDiff == -1) && (($board[sX-1][sY-1].image != $tileBrown && $board[sX-1][sY-1].image != $tileBlack) || isPassant)) then
			return true
		end
		if ((xDiff == -1 && yDiff == 1) && (($board[sX-1][sY+1].image != $tileBrown && $board[sX-1][sY+1].image != $tileBlack) || isPassant)) then
			return true
		end
	else
		#are we moving the wrong way?
		if (xDiff < -1 || xDiff == -1) then
			return false
		end
		#is it first move?
		if (sX == 1) then
			#is a two square move being attempted?
			if (xDiff == 2) then
				#is it blocked?
				if xCollision then
					return false
				end
				if(($board[sX+2][sY].image == $tileBrown || $board[sX+2][sY].image == $tileBlack) && yDiff == 0) then
					return true
				end
			end
			if ((xDiff == 1 && yDiff == 0) && ($board[sX+1][sY].image == $tileBrown || $board[sX+1][sY].image == $tileBlack)) then
				return true
			end
		end		
		#are we capturing?
		if ((xDiff == 1 && yDiff == -1) && (($board[sX+1][sY-1].image != $tileBrown && $board[sX+1][sY-1].image != $tileBlack) || isPassant))then
			return true
		end
		if ((xDiff == 1 && yDiff == 1) && (($board[sX+1][sY+1].image != $tileBrown && $board[sX+1][sY+1].image != $tileBlack) || isPassant))then
			return true
		end
	end	
	return false
end

def isPassant
	
	return false
end

def xyCollision
	xInc = $secondClickX - $firstClickX
	yInc = $secondClickY - $firstClickY
	if ((xInc == 1 || xInc == -1) && (yInc == 1 || yInc == -1)) then
		return false
	end
	xNeg = 1
	yNeg = 1	
	if (xInc < 0) then
		xNeg = -1
	end
	if (yInc < 0) then
		yNeg = -1
	end
	xInc = (xInc/xInc)*xNeg
	yInc = (yInc/yInc)*yNeg
	curX = $firstClickX + xInc
	curY = $firstClickY + yInc
	while(curX != $secondClickX && curY != $secondClickY)
		if ($board[curX][curY].image == $tileBrown || $board[curX][curY].image == $tileBlack) then
			
		else
			return true
		end
		curX = curX + xInc
		curY = curY + yInc
	end
	return false
end

def xCollision
	xInc = $secondClickX - $firstClickX
	xNeg = 1
	if (xInc == 1 || xInc == -1) then
		return false
	end
	if (xInc < 0) then
		xNeg = -1
	end
	xInc = (xInc/xInc)*xNeg
	curX = $firstClickX + xInc
	while(curX != $secondClickX)
		if ($board[curX][$firstClickY].image == $tileBrown || $board[curX][$firstClickY].image == $tileBlack) then
			
		else
			return true
		end
		curX = curX + xInc
	end	
	return false
end

def yCollision
	yInc = $secondClickY - $firstClickY
	yNeg = 1
	if (yInc == 1 || yInc == -1) then
		return false
	end
	if (yInc < 0) then
		yNeg = -1
	end
	yInc = (yInc/yInc)*yNeg
	curY = $firstClickY + yInc	
	while (curY != $secondClickY)
		if ($board[$firstClickX][curY].image == $tileBrown || $board[$firstClickX][curY].image == $tileBlack) then
			
		else
			return true
		end
		curY = curyY + yInc
	end	
	return false
end

def swapPawn
	#create additional window prompt with piece selections
	
end

def whiteInCheck
	firstXBkp = $firstClickX
	firstYBkp = $firstClickY
	secondXBkp = $secondClickX
	secondYBkp = $secondClickY
	whiteKingX = 0
	whiteKingY = 0
	curX = 0
	curY = 0
	whiteKingInCheck = false
	#iterate through all squares with destination as whiteKing
	$assessingCheck = true
	#find king
	while (curX < 7) 
		while (curY < 7)
			if ($board[curX][curY].image == $whiteKing) then
				whiteKingX=curX
				whiteKingY=curY
			end
		end
	end
	$secondClickX = whiteKingX
	$secondClickY = whiteKingY
	curX = 0
	curY = 0
	#check all moves to king loc
	while (curX < 7) 
		while (curY < 7)
			$firstClickX = curX
			$firstClickY = curY	
			if validateMove then
				whiteKingInCheck = true
			end
		end
	end
	$assessingCheck = false
	$firstClickX = firstXBkp
	$firstClickY = firstYBkp
	$secondClickX = secondXBkp
	$secondClickY = secondYBkp
	return whiteKingInCheck
end

def blackInCheck
	firstXBkp = $firstClickX
	firstYBkp = $firstClickY
	secondXBkp = $secondClickX
	secondYBkp = $secondClickY
	blackKingX = 0
	blackKingY = 0
	curX = 0
	curY = 0
	blackKingInCheck = false
	#iterate through all squares with destination as blackKing
	$assessingCheck = true
	#find king
	while (curX < 7) 
		while (curY < 7)
			if ($board[curX][curY].image == $blackKing) then
				blackKingX=curX
				blackKingY=curY
			end
		end
	end
	$secondClickX = blackKingX
	$secondClickY = blackKingY
	curX = 0
	curY = 0
	#check all moves to king loc
	while (curX < 7) 
		while (curY < 7)
			$firstClickX = curX
			$firstClickY = curY
			if validateMove then
				blackKingInCheck = true
			end
		end
	end
	$assessingCheck = false
	$firstClickX = firstXBkp
	$firstClickY = firstYBkp
	$secondClickX = secondXBkp
	$secondClickY = secondYBkp
	return blackKingInCheck
end

Tk.mainloop