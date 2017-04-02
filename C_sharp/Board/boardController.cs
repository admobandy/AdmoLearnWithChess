using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Board
    
//a brief note on this class
//This is the backend engine for a chess-game application it is only an environment constructor and manipulator
//The following items are supported:
//move validation
//check validation
//pawn replacement
//board state retrieval
//The following items are not supported:
//turn validation
//automatic calls to inCheck and pawnReplace
{
    class boardController
    {
        string[] boardArray = new string[64];
        bool whiteKingMove, blackKingMove, whiteInCheck, blackInCheck, bkrMove, bqrMove, wqrMove, wkrMove, pawnCross;
        int previousPieceLocation, previousDestination;
        string previousMovedPiece, tempPiece;
        int movementDifference, pieceRow, pieceColumn, destinationRow, destinationColumn;
        int n, m, p, multipleOfSeven, multipleOfNine, moveStatus;
        string tempLoc, tempDest;
        int blackKingLoc, whiteKingLoc;

        public boardController()
        {               
            //set special move conditionals
            pawnCross = false;
            previousMovedPiece = " E ";
            previousPieceLocation = -1;
            previousDestination = -1;
            whiteKingMove = false;
            blackKingMove = false;
            blackInCheck = false;
            whiteInCheck = false;
            bkrMove = false;
            bqrMove = false;
            wqrMove = false;
            wkrMove = false;
            blackKingLoc = 4;
            whiteKingLoc = 60;

            //place black pieces
            boardArray[0] = "BQR";
            boardArray[1] = "BQN";
            boardArray[2] = "BQB";
            boardArray[3] = "BQQ";
            boardArray[4] = "BKK";
            boardArray[5] = "BKB";
            boardArray[6] = "BKN";
            boardArray[7] = "BKR";
            boardArray[8] = "BQP1";
            boardArray[9] = "BQP2";
            boardArray[10] = "BQP3";
            boardArray[11] = "BQP4";
            boardArray[12] = "BKP1";
            boardArray[13] = "BKP2";
            boardArray[14] = "BKP3";
            boardArray[15] = "BKP4";

            //place white pieces
            boardArray[48] = "WQP1";
            boardArray[49] = "WQP2";
            boardArray[50] = "WQP3";
            boardArray[51] = "WQP4";
            boardArray[52] = "WKP1";
            boardArray[53] = "WKP2";
            boardArray[54] = "WKP3";
            boardArray[55] = "WKP4";
            boardArray[56] = "WQR";
            boardArray[57] = "WQN";
            boardArray[58] = "WQB";
            boardArray[59] = "WQQ";
            boardArray[60] = "WKK";
            boardArray[61] = "WKB";
            boardArray[62] = "WKN";
            boardArray[63] = "WKR";

            //place 'empty' in remaining squares    
            p = 16;
            while (p < 48)
            {
                boardArray[p] = " E ";
                p++;
            }
        }

        public string getElement(int num)
        {
            return boardArray[num];
        }

        public void setElement(int n, string s)
        {
            boardArray[n] = s;
            return;
        }

        //returns 1 if black is in check and -1 if white is in check and 0 if there is no check state on the board
        public int inCheck()
        {
        blackInCheck = false;
        whiteInCheck = false;
    
        //check black
            //see if any piece has a valid move to the black kings location
            m = 0;
            while(m < 64){  
                tempPiece = boardArray[m];
                moveStatus = tryMove(m,blackKingLoc);                          
                if(moveStatus == 1){  
                    boardArray[m] = tempPiece;
                    boardArray[blackKingLoc] = "BKK";
                    blackInCheck = true;         
                }
                m++;
            }
        //check white             
            //see if any piece has a valid move to the white kings location
            
            m = 0;
            while(m < 64){ 
                tempPiece = boardArray[m];  
                moveStatus = tryMove(m,whiteKingLoc);        
                if(moveStatus == 1){  
                    boardArray[m] = tempPiece;
                    boardArray[blackKingLoc] = "WKK";              
                    whiteInCheck = true;
                }
                m++;
            }
            
            boardArray[blackKingLoc] = "BKK";
            boardArray[whiteKingLoc] = "WKK";
            if(whiteInCheck == true){
              return -1;
            }

            if(blackInCheck == true){
              return 1;
            }
    
            return 0;
        }

        
    //must be called when tryMove returns -1
    //pieceSelection must be queen, roook, knight, or bishop and be labeled as a queen piece of the appropriate color
    //returns 1 if completed successfully, else returns 0
    public int pawnReplace(string pieceSelection)
    {
    //validate function call    
    if(pawnCross){    
    //check for valid piece selection
            //black
            if((previousDestination > 55) && (previousDestination < 64)){
                if(pieceSelection == "Q"){
                    boardArray[previousDestination]="BQQ"; 
                    pawnCross = false;   
                    return 1;
                }

                if(pieceSelection == "R"){
                    boardArray[previousDestination]="BQR";
                    pawnCross = false;
                    return 1;
                }

                if(pieceSelection == "N"){
                    boardArray[previousDestination]="BQN";
                    pawnCross = false;
                    return 1;
                }

                if(pieceSelection == "B"){
                    boardArray[previousDestination]="BQB";
                    pawnCross = false;
                    return 1;
                }     
                return 0;                                   
            }
            //white
            if((previousDestination > -1) && (previousDestination < 8)){
                if(pieceSelection == "Q"){
                    boardArray[previousDestination]="WQQ";   
                    pawnCross = false; 
                    return 1;
                }

                if(pieceSelection == "R"){
                    boardArray[previousDestination]="WQR";
                    pawnCross = false;
                    return 1;
                }

                if(pieceSelection == "N"){
                    boardArray[previousDestination]="WQN";
                    pawnCross = false;
                    return 1;
                }

                if(pieceSelection == "B"){
                    boardArray[previousDestination]="WQB";
                    pawnCross = false;
                    return 1;
                } 
                return 0;                                 
            }            
        }
        return 0;
    }

    //returns 1 if move is valid and board-state has been changed, else returns 0 and the move is not valid
//if a pawn has reached the other side of the board it returns a -1 to signal that a piece replacement must be made
public int tryMove(int pieceLocation, int destination)
{
    //check for invalid input
    if((destination < 0) || (destination>63)){
                    return 0;                
    }
    //check for self-capture 
    tempLoc = boardArray[pieceLocation].Remove(1);
    tempDest = boardArray[destination].Remove(1);

    if(tempLoc == " "){
        return 0;
    }
    

    if(tempLoc == tempDest){
        return 0;
    }


    if(pawnCross == true){
        return -1;
    }


    if(boardArray[pieceLocation] == " E " ){
        return 0;
    }

    
    if(pieceLocation == destination){
                    return 0;
    }    

    string movingPiece = boardArray[pieceLocation];
    string destinationSquare = boardArray[destination];
    movementDifference = destination - pieceLocation;
    
    pieceColumn = pieceLocation % 8;
    pieceRow = ((pieceLocation/8)%8);
    destinationColumn = destination % 8;
    destinationRow = ((destination/8)%8);      
        
    //check for castle
        //black castle
        if(movingPiece == "BKK"){
            //king side
            if(pieceLocation == 4 && destination == 6 && blackKingMove == false && bkrMove == false){
                //ensure move path is empty
                if(boardArray[5] == " E " && boardArray[6] == " E "){
                    if(tryMove(4,5) == 1){
                        if(tryMove(5,6) == 1){
                            boardArray[5] = "BKR";
                            boardArray[7] = " E ";
                            return 1;
                        }
                    }
                    boardArray[4] = "BKK";
                    blackKingMove = false;
                    blackKingLoc = 4;
                    return 0;
                }
            }
            //queen side
            if(pieceLocation == 4 && destination == 2 && blackKingMove == false && bqrMove == false){
                //ensure move path is empty
                if(boardArray[1] == " E " && boardArray[2] == " E " && boardArray[3] == " E "){
                    if(tryMove(4,3) == 1){
                        if(tryMove(3,2) == 1){
                            boardArray[3] = "BQR";
                            boardArray[0] = " E ";
                            return 1;
                        }
                    }  
                    boardArray[4] = "BKK";
                    blackKingMove = false;
                    blackKingLoc = 4;
                    return 0;
                }
            }
        }
        //white castle  
        if(movingPiece == "WKK"){
            //king side
            if(pieceLocation == 60 && destination == 62 && whiteKingMove == false && wkrMove == false){
                //ensure move path is empty
                if(boardArray[61] == " E " && boardArray[62] == " E "){
                    if(tryMove(60,61) == 1){
                        if(tryMove(61,62) == 1){
                            boardArray[63] = " E ";
                            boardArray[61] = "WKR";
                            return 1;
                        }
                    }    
                    boardArray[60] = "WKK";
                    whiteKingMove = false;
                    whiteKingLoc = 60;
                    return 0;
                }
            }
            //queen side
            if(pieceLocation == 60 && destination == 58 && whiteKingMove == false && wqrMove == false){
                //ensure move path is empty
                if(boardArray[59] == " E " && boardArray[58] == " E " && boardArray[57] == " E "){
                    if(tryMove(60,59) == 1){
                        if(tryMove(59,58) == 1){
                            boardArray[59] = "WQR";
                            boardArray[56] = " E ";
                            return 1;    
                        }
                    }
                    boardArray[60] = "WKK";
                    whiteKingMove = false;
                    whiteKingLoc = 60;
                    return 0;
                }
            }
        }

    //check for piece collisions along destination path unless piece is a Knight
    if(((movingPiece == "BQN") || (movingPiece == "BKN") || (movingPiece == "WQN") || (movingPiece == "WKN"))){
                   goto moveValidation;     
    }
        
            //check to see if destination is in same row or column, or along a diagonal
            //traverse path checking for collisions until destination is reached or a collision occurs
            //if no collision then jump to validation
            //row collision check
            if(pieceRow == destinationRow){
                        if(movementDifference > 0){
                        //moving to the right   
                                 n = pieceLocation + 1;
                                 while(n < destination){
                                         if(boardArray[n]!=" E "){
                                              return 0;     
                                         }
                                         n++;
                                 } 
                        }
                        if(movementDifference < 0){
                        //moving to the left   
                                 n = pieceLocation - 1;
                                 while(n > destination){
                                         if(boardArray[n]!=" E "){
                                              return 0;     
                                         }
                                        n--;         
                                 }
                        }
            }
            
            //column collision check
            if(pieceColumn == destinationColumn){
                        if(movementDifference > 0){
                        //moving down        
                                 n = pieceLocation + 8;
                                 while(n < destination){
                                         if(boardArray[n]!=" E "){
                                              return 0;     
                                         }
                                         n=n+8;
                                 }
                        }
                        if(movementDifference < 0){
                        //moving up    
                                 n = pieceLocation - 8;
                                 while(n > destination){
                                         if(boardArray[n]!=" E "){
                                              return 0;     
                                         }
                                        n=n-8;         
                                 }
                        }    
            }
            
            //diagonal check     
            //if movement difference is a multiple of 7 or 9 and is negative or positive            
            multipleOfSeven = movementDifference % 7;
            multipleOfNine = movementDifference % 9;

            //check for illegal diagonal move
                                    
            //multiple of 7 and positive
            if((multipleOfSeven == 0)&&(movementDifference > 0)){
                                if(destinationColumn >= pieceColumn){
                                    return 0;
                                }
                                n = pieceLocation + 7;
                                while(n < destination){
                                        if(boardArray[n]!=" E "){
                                              return 0;                    
                                        }
                                        n = n + 7;        
                                }  
            }
            
            //multiple of 7 and negative
            if((multipleOfSeven == 0)&&(movementDifference < 0)){
                                if(destinationColumn <= pieceColumn){
                                    return 0;
                                }
                                n = pieceLocation - 7;
                                while(n > destination){
                                        if(boardArray[n]!=" E "){
                                              return 0;                    
                                        }
                                        n = n - 7;        
                                }  
            }
            
            //multiple of 9 and positive
            if((multipleOfNine == 0)&&(movementDifference > 0)){
                                if(destinationColumn <= pieceColumn){
                                    return 0;
                                }
                               n = pieceLocation + 9;
                               while(n < destination){
                                        if(boardArray[n]!=" E "){
                                              return 0;                    
                                        }
                                        n = n + 9;        
                                }   
            }
            
            //multiple of 9 and negative
            if((multipleOfNine ==0)&&(movementDifference < 0)){
                                if(destinationColumn >= pieceColumn){
                                    return 0;
                                }
                               n = pieceLocation - 9;
                               while(n > destination){
                                        if(boardArray[n]!=" E "){
                                              return 0;                    
                                        }
                                        n = n - 9;        
                                }   
            }
            
    //validate move
    moveValidation:
    //BQR
         
         if(movingPiece == "BQR" ){
                        if(pieceRow == destinationRow){
                                    bqrMove = true;
                                    goto validMove;            
                        }
                        if(pieceColumn == destinationColumn){
                                    bqrMove = true;   
                                    goto validMove;   
                        }
                        return 0;                              
         }
    //BQN
         
         if(movingPiece == "BQN" ){
                        //valid moves are +-6,10,15,17 unless in columns 0,1,6,7
                        if(pieceColumn == 0){
                                       //no 6,15,-10,-17
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }  
                                       if(movementDifference == 15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -17){
                                                             return 0;                      
                                       }          
                        }
                        
                        if(pieceColumn == 1){
                                       //no 6,-10
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 6){
                                       //no -6,10
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 7){
                                       //no -6,-15,10,17
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 17){
                                                             return 0;                      
                                       }
                                       
                        }             
                                                
                        if((movementDifference==6)||(movementDifference==-6)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==10)||(movementDifference==-10)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==15)||(movementDifference==-15)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==17)||(movementDifference==-17)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }  
                        return 0;                                     
         }
    //BQB
         
         if(movingPiece == "BQB" ){
                        //valid moves multiples of +-9,7 unless in columns 0,7   
                        //check to make sure destination is not same row or column
                        if(pieceRow == destinationRow){
                                    return 0;            
                        }
                        
                        if(pieceColumn == destinationColumn){
                                    return 0;               
                        }
                        
                        //check column 0 and 7 cases
                        if(pieceColumn == 7){
                                       //no -7,-14,-21,-28,-35,-42,-49,-56
                                       //9,18,27,36,45,54
                                       if(movementDifference==-7){
                                                return 0;                         
                                       }
                                       if(movementDifference==-14){
                                                return 0;                         
                                       }
                                       if(movementDifference==-21){
                                                return 0;                         
                                       }
                                       if(movementDifference==-28){
                                                return 0;                         
                                       }
                                       if(movementDifference==-35){
                                                return 0;                         
                                       }
                                       if(movementDifference==-42){
                                                return 0;                         
                                       }
                                       if(movementDifference==-49){
                                                return 0;                         
                                       }
                                       if(movementDifference==-56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==9){
                                                return 0;                         
                                       }
                                       if(movementDifference==18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==54){
                                                return 0;                         
                                       }              
                        }
                        
                        if(pieceColumn == 0){
                                       //no -9,-18,-27,-36,-45,-54
                                       //7,14,21,28,35,42,49,56
                                       if(movementDifference==7){
                                                return 0;                         
                                       }
                                       if(movementDifference==14){
                                                return 0;                         
                                       }
                                       if(movementDifference==21){
                                                return 0;                         
                                       }
                                       if(movementDifference==28){
                                                return 0;                         
                                       }
                                       if(movementDifference==35){
                                                return 0;                         
                                       }
                                       if(movementDifference==42){
                                                return 0;                         
                                       }
                                       if(movementDifference==49){
                                                return 0;                         
                                       }
                                       if(movementDifference==56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==-9){
                                                return 0;                         
                                       }
                                       if(movementDifference==-18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-54){
                                                return 0;                         
                                       }
                        }      
                        
                        //general validation, movement difference is a multiple of 7 or 9
                        
                        if(movementDifference == 7){
                           
                                              goto validMove;
                        }
                        if(movementDifference == 14){
                            
                                              goto validMove;
                        }
                        if(movementDifference == 21){
                           
                                              goto validMove;
                        }
                        if(movementDifference == 28){
                            
                                              goto validMove;
                        }
                        if(movementDifference == 35){
                            
                                              goto validMove;
                        }
                        if(movementDifference == 42){
                            
                                              goto validMove;
                        }
                        if(movementDifference == 49){
                            
                                              goto validMove;
                        }
                        if(movementDifference == 56){
                            
                                              goto validMove;
                        } 
                        if(movementDifference == -7){
                            
                                              goto validMove;
                        } 
                        if(movementDifference == -14){
                            
                                              goto validMove;
                        }
                        if(movementDifference == -21){
                            
                                              goto validMove;
                        }
                        if(movementDifference == -28){
                            
                                              goto validMove;
                        }
                        if(movementDifference == -35){
                            
                                              goto validMove;
                        }
                        if(movementDifference == -42){
                            
                                              goto validMove;
                        }
                        if(movementDifference == -49){
                            
                                              goto validMove;
                        }
                        if(movementDifference == -56){
                            
                                              goto validMove;
                        }
                        if(movementDifference == 9){
                                
                                              goto validMove;
                        } 
                        if(movementDifference == 18){
                                
                                              goto validMove;
                        }
                        if(movementDifference == 27){
                                
                                              goto validMove;
                        }
                        if(movementDifference == 36){
                                
                                              goto validMove;
                        }
                        if(movementDifference == 45){
                                
                                              goto validMove;
                        }
                        if(movementDifference == 54){
                                
                                              goto validMove;
                        }
                        if(movementDifference == -9){
                                
                                              goto validMove;
                        } 
                        if(movementDifference == -18){
                                
                                              goto validMove;
                        }
                        if(movementDifference == -27){
                                
                                              goto validMove;
                        }
                        if(movementDifference == -36){
                                
                                              goto validMove;
                        }
                        if(movementDifference == -45){
                                
                                              goto validMove;
                        }
                        if(movementDifference == -54){
                                
                                              goto validMove;
                        }
                        return 0;     
         }
    //BQ
         
         if(movingPiece == "BQQ" ){
                        //if move is along a diagonal,row or column it is valid
                        //check rook-like movement
                        if(pieceRow == destinationRow){
                                    goto validMove;            
                        }
                        if(pieceColumn == destinationColumn){
                                    goto validMove;   
                        }
                        
                        //check column 0 and 7 cases
                        if(pieceColumn == 7){
                                       //no -7,-14,-21,-28,-35,-42,-49,-56
                                       //9,18,27,36,45,54
                                       if(movementDifference==-7){
                                                return 0;                         
                                       }
                                       if(movementDifference==-14){
                                                return 0;                         
                                       }
                                       if(movementDifference==-21){
                                                return 0;                         
                                       }
                                       if(movementDifference==-28){
                                                return 0;                         
                                       }
                                       if(movementDifference==-35){
                                                return 0;                         
                                       }
                                       if(movementDifference==-42){
                                                return 0;                         
                                       }
                                       if(movementDifference==-49){
                                                return 0;                         
                                       }
                                       if(movementDifference==-56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==9){
                                                return 0;                         
                                       }
                                       if(movementDifference==18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==54){
                                                return 0;                         
                                       }              
                        }
                        
                        if(pieceColumn == 0){
                                       //no -9,-18,-27,-36,-45,-54
                                       //7,14,21,28,35,42,49,56
                                       if(movementDifference==7){
                                                return 0;                         
                                       }
                                       if(movementDifference==14){
                                                return 0;                         
                                       }
                                       if(movementDifference==21){
                                                return 0;                         
                                       }
                                       if(movementDifference==28){
                                                return 0;                         
                                       }
                                       if(movementDifference==35){
                                                return 0;                         
                                       }
                                       if(movementDifference==42){
                                                return 0;                         
                                       }
                                       if(movementDifference==49){
                                                return 0;                         
                                       }
                                       if(movementDifference==56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==-9){
                                                return 0;                         
                                       }
                                       if(movementDifference==-18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-54){
                                                return 0;                         
                                       }
                        }      
                        
                        //general validation, movement difference is a multiple of 7 or 9
                        
                        if(movementDifference == 7){
                                              goto validMove;
                        }
                        if(movementDifference == 14){
                                              goto validMove;
                        }
                        if(movementDifference == 21){
                                              goto validMove;
                        }
                        if(movementDifference == 28){
                                              goto validMove;
                        }
                        if(movementDifference == 35){
                                              goto validMove;
                        }
                        if(movementDifference == 42){
                                              goto validMove;
                        }
                        if(movementDifference == 49){
                                              goto validMove;
                        }
                        if(movementDifference == 56){
                                              goto validMove;
                        } 
                        if(movementDifference == -7){
                                              goto validMove;
                        } 
                        if(movementDifference == -14){
                                              goto validMove;
                        }
                        if(movementDifference == -21){
                                              goto validMove;
                        }
                        if(movementDifference == -28){
                                              goto validMove;
                        }
                        if(movementDifference == -35){
                                              goto validMove;
                        }
                        if(movementDifference == -42){
                                              goto validMove;
                        }
                        if(movementDifference == -49){
                                              goto validMove;
                        }
                        if(movementDifference == -56){
                                              goto validMove;
                        }
                        if(movementDifference == 9){
                                              goto validMove;
                        } 
                        if(movementDifference == 18){
                                              goto validMove;
                        }
                        if(movementDifference == 27){
                                              goto validMove;
                        }
                        if(movementDifference == 36){
                                              goto validMove;
                        }
                        if(movementDifference == 45){
                                              goto validMove;
                        }
                        if(movementDifference == 54){
                                              goto validMove;
                        }
                        if(movementDifference == -9){
                                              goto validMove;
                        } 
                        if(movementDifference == -18){
                                              goto validMove;
                        }
                        if(movementDifference == -27){
                                              goto validMove;
                        }
                        if(movementDifference == -36){
                                              goto validMove;
                        }
                        if(movementDifference == -45){
                                              goto validMove;
                        }
                        if(movementDifference == -54){
                                              goto validMove;
                        }
                        return 0;
         }
    //BK
         
         if(movingPiece == "BKK" ){
                                                
                        //vertical movement
                        if((movementDifference == 8 || movementDifference == -8) && pieceColumn == destinationColumn){
                            blackKingLoc = destination;   
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == 1){
                                boardArray[pieceLocation] = "BKK";
                                boardArray[destination] = destinationSquare;
                                blackKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece; 
                            blackKingMove = true;
                            return 1;   
                        }
                        //horizontal movement
                        if((movementDifference == 1 || movementDifference == -1) && pieceRow == destinationRow){
                            blackKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == 1){
                                boardArray[pieceLocation] = "BKK";
                                boardArray[destination] = destinationSquare;
                                blackKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            blackKingMove = true;
                            return 1;
                        }

                        //diagonal movement
                        // -7
                        if((movementDifference == -7) && (pieceRow - destinationRow == 1) && (pieceColumn - destinationColumn == 1)){
                            blackKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == 1){
                                boardArray[pieceLocation] = "BKK";
                                boardArray[destination] = destinationSquare;
                                blackKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            blackKingMove = true;
                            return 1;
                        }
                        //7
                        if((movementDifference == 7) && (pieceRow - destinationRow == -1) && (pieceColumn - destinationColumn == 1)){
                            blackKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == 1){
                                boardArray[pieceLocation] = "BKK";
                                boardArray[destination] = destinationSquare;
                                blackKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            blackKingMove = true;
                            return 1;
                        }
                        //-9
                        if((movementDifference == -9) && (pieceRow - destinationRow == 1) && (pieceColumn - destinationColumn == 1)){
                            blackKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == 1){
                                boardArray[pieceLocation] = "BKK";
                                boardArray[destination] = destinationSquare;
                                blackKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            blackKingMove = true;
                            return 1;
                        }
                        //9
                        if((movementDifference == 9) && (pieceRow - destinationRow == -1) && (pieceColumn - destinationColumn == -1)){
                            blackKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == 1){
                                boardArray[pieceLocation] = "BKK";
                                boardArray[destination] = destinationSquare;
                                blackKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            blackKingMove = true;
                            return 1;
                        }

                        
                        return 0;
         }
    //BKB
         
         if(movingPiece == "BKB" ){
                        //same as BQB 
                        if(pieceRow == destinationRow){
                                    return 0;            
                        }
                        
                        if(pieceColumn == destinationColumn){
                                    return 0;               
                        }
                        
                        //check column 0 and 7 cases
                        if(pieceColumn == 7){
                                       //no -7,-14,-21,-28,-35,-42,-49,-56
                                       //9,18,27,36,45,54
                                       if(movementDifference==-7){
                                                return 0;                         
                                       }
                                       if(movementDifference==-14){
                                                return 0;                         
                                       }
                                       if(movementDifference==-21){
                                                return 0;                         
                                       }
                                       if(movementDifference==-28){
                                                return 0;                         
                                       }
                                       if(movementDifference==-35){
                                                return 0;                         
                                       }
                                       if(movementDifference==-42){
                                                return 0;                         
                                       }
                                       if(movementDifference==-49){
                                                return 0;                         
                                       }
                                       if(movementDifference==-56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==9){
                                                return 0;                         
                                       }
                                       if(movementDifference==18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==54){
                                                return 0;                         
                                       }              
                        }
                        
                        if(pieceColumn == 0){
                                       //no -9,-18,-27,-36,-45,-54
                                       //7,14,21,28,35,42,49,56
                                       if(movementDifference==7){
                                                return 0;                         
                                       }
                                       if(movementDifference==14){
                                                return 0;                         
                                       }
                                       if(movementDifference==21){
                                                return 0;                         
                                       }
                                       if(movementDifference==28){
                                                return 0;                         
                                       }
                                       if(movementDifference==35){
                                                return 0;                         
                                       }
                                       if(movementDifference==42){
                                                return 0;                         
                                       }
                                       if(movementDifference==49){
                                                return 0;                         
                                       }
                                       if(movementDifference==56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==-9){
                                                return 0;                         
                                       }
                                       if(movementDifference==-18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-54){
                                                return 0;                         
                                       }
                        }      
                        
                        //general validation, movement difference is a multiple of 7 or 9
                        
                        if(movementDifference == 7){
                                              goto validMove;
                        }
                        if(movementDifference == 14){
                                              goto validMove;
                        }
                        if(movementDifference == 21){
                                              goto validMove;
                        }
                        if(movementDifference == 28){
                                              goto validMove;
                        }
                        if(movementDifference == 35){
                                              goto validMove;
                        }
                        if(movementDifference == 42){
                                              goto validMove;
                        }
                        if(movementDifference == 49){
                                              goto validMove;
                        }
                        if(movementDifference == 56){
                                              goto validMove;
                        } 
                        if(movementDifference == -7){
                                              goto validMove;
                        } 
                        if(movementDifference == -14){
                                              goto validMove;
                        }
                        if(movementDifference == -21){
                                              goto validMove;
                        }
                        if(movementDifference == -28){
                                              goto validMove;
                        }
                        if(movementDifference == -35){
                                              goto validMove;
                        }
                        if(movementDifference == -42){
                                              goto validMove;
                        }
                        if(movementDifference == -49){
                                              goto validMove;
                        }
                        if(movementDifference == -56){
                                              goto validMove;
                        }
                        if(movementDifference == 9){
                                              goto validMove;
                        } 
                        if(movementDifference == 18){
                                              goto validMove;
                        }
                        if(movementDifference == 27){
                                              goto validMove;
                        }
                        if(movementDifference == 36){
                                              goto validMove;
                        }
                        if(movementDifference == 45){
                                              goto validMove;
                        }
                        if(movementDifference == 54){
                                              goto validMove;
                        }
                        if(movementDifference == -9){
                                              goto validMove;
                        } 
                        if(movementDifference == -18){
                                              goto validMove;
                        }
                        if(movementDifference == -27){
                                              goto validMove;
                        }
                        if(movementDifference == -36){
                                              goto validMove;
                        }
                        if(movementDifference == -45){
                                              goto validMove;
                        }
                        if(movementDifference == -54){
                                              goto validMove;
                        }
                        return 0;              
         }
    //BKN
         
         if(movingPiece == "BKN" ){
                        //same as BQN  
                        if(pieceColumn == 0){
                                       //no 6,15,-10,-17
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }  
                                       if(movementDifference == 15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -17){
                                                             return 0;                      
                                       }          
                        }
                        
                        if(pieceColumn == 1){
                                       //no 6,-10
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 6){
                                       //no -6,10
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 7){
                                       //no -6,-15,10,17
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 17){
                                                             return 0;                      
                                       }
                                       
                        }             
                                                
                        if((movementDifference==6)||(movementDifference==-6)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==10)||(movementDifference==-10)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==15)||(movementDifference==-15)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==17)||(movementDifference==-17)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }  
                        return 0;           
         }
    //BKR
         
         if(movingPiece == "BKR" ){
                        if(pieceRow == destinationRow){
                                    bkrMove = true;
                                    goto validMove;            
                        }
                        if(pieceColumn == destinationColumn){
                                    bkrMove = true;   
                                    goto validMove;   
                        }
                        return 0;               
         }
    //BKP

         if ((movingPiece == "BKP1") || (movingPiece == "BKP2") || (movingPiece == "BKP3") || (movingPiece == "BKP4"))
         {
                        //must only advance along column unless capturing at a forward diagonal, movement is +8
                        //check for crossing the board
                        if((pieceRow == 6) && (destination > 55) && (destination < 64)){
                                     //pawn has crossed the board and a new piece must be selected
                                     pawnCross = true;                                     
                        }
                        
                        
                        //check for capture
                        if(((movementDifference == 7) || (movementDifference == 9))){
                                                if((destinationColumn - pieceColumn) == 1){
                                                                      //check for passant
                                                                     if((previousPieceLocation > 47) && (previousPieceLocation < 56) && (previousDestination > 31) && (previousDestination < 40)){
                                                                         if ((previousMovedPiece == "WKP1") || (previousMovedPiece == "WQP1") || (previousMovedPiece == "WKP2") || (previousMovedPiece == "WQP2") || (previousMovedPiece == "WKP3") || (previousMovedPiece == "WQP3") || (previousMovedPiece == "WKP4") || (previousMovedPiece == "WQP4"))
                                                                         {
                                                                                                                      if((destination - previousDestination == 8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     } 
                                                                     if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }    
                                                }
                                                if((destinationColumn - pieceColumn)== -1){
                                                                      //check for passant
                                                                      if((previousPieceLocation > 47) && (previousPieceLocation < 56) && (previousDestination > 31) && (previousDestination < 40)){
                                                                          if ((previousMovedPiece == "WKP1") || (previousMovedPiece == "WQP1") || (previousMovedPiece == "WKP2") || (previousMovedPiece == "WQP2") || (previousMovedPiece == "WKP3") || (previousMovedPiece == "WQP3") || (previousMovedPiece == "WKP4") || (previousMovedPiece == "WQP4"))
                                                                          {
                                                                                                                      if((destination - previousDestination == 8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     }
                                                                      if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }   
                                                }
                        } 
                        //check for advancing 2 squares on first move                        
                        if(pieceRow == 1){
                                       if((pieceColumn == destinationColumn) && ((destination - pieceLocation == 8) || (destination - pieceLocation == 16))){
                                                       goto validMove;               
                                       }                    
                        }
                        //regular pawn movement
                        if((pieceColumn == destinationColumn) && (destination - pieceLocation == 8)){
                                        goto validMove;               
                        }               
                        return 0;           
         }
    //BQP

         if ((movingPiece == "BQP1") || (movingPiece == "BQP2") || (movingPiece == "BQP3") || (movingPiece == "BQP4"))
         {
                        //same as BKP
                        //check for crossing the board
                        if((pieceRow == 6) && (destination > 55) && (destination < 64)){
                                     //pawn has crossed the board and a new piece must be selected
                                     pawnCross = true;                                     
                        }
                        
                        
                        //check for capture
                        if(((movementDifference == 7) || (movementDifference == 9))){
                                                if((destinationColumn - pieceColumn) == 1){
                                                                      //check for passant
                                                                     if((previousPieceLocation > 47) && (previousPieceLocation < 56) && (previousDestination > 31) && (previousDestination < 40)){
                                                                                               if((previousMovedPiece == "WKP1") || (previousMovedPiece == "WQP1") || (previousMovedPiece == "WKP2") || (previousMovedPiece == "WQP2") || (previousMovedPiece == "WKP3") || (previousMovedPiece == "WQP3") || (previousMovedPiece == "WKP4") || (previousMovedPiece == "WQP4")){
                                                                                                                      if((destination - previousDestination == 8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     } 
                                                                     if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }    
                                                }
                                                if((destinationColumn - pieceColumn)== -1){
                                                                      //check for passant
                                                                      if((previousPieceLocation > 47) && (previousPieceLocation < 56) && (previousDestination > 31) && (previousDestination < 40)){
                                                                                               if((previousMovedPiece == "WKP1") || (previousMovedPiece == "WQP1") || (previousMovedPiece == "WKP2") || (previousMovedPiece == "WQP2") || (previousMovedPiece == "WKP3") || (previousMovedPiece == "WQP3") || (previousMovedPiece == "WKP4") || (previousMovedPiece == "WQP4")){
                                                                                                                      if((destination - previousDestination == 8) && (boardArray[destination] == " E ")){
                                                                                                                                      boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     }
                                                                      if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }   
                                                }
                        }
                        //check for advancing 2 squares on first move
                        if(pieceRow == 1){
                                       if((pieceColumn == destinationColumn) && ((destination - pieceLocation == 8) || (destination - pieceLocation == 16))){
                                                       goto validMove;               
                                       }                    
                        }
                        //regular pawn movement
                        if((pieceColumn == destinationColumn) && (destination - pieceLocation == 8)){
                                        goto validMove;               
                        }               
                        return 0;
         }
    //WQR
         
         if(movingPiece == "WQR" ){
                        if(pieceRow == destinationRow){
                                    wqrMove = true;
                                    goto validMove;            
                        }
                        if(pieceColumn == destinationColumn){
                                    wqrMove = true;   
                                    goto validMove;   
                        }
                        return 0;               
         }
    //WQN
         
         if(movingPiece == "WQN" ){
                        //same as BQN 
                        if(pieceColumn == 0){
                                       //no 6,15,-10,-17
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }  
                                       if(movementDifference == 15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -17){
                                                             return 0;                      
                                       }          
                        }
                        
                        if(pieceColumn == 1){
                                       //no 6,-10
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 6){
                                       //no -6,10
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 7){
                                       //no -6,-15,10,17
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 17){
                                                             return 0;                      
                                       }
                                       
                        }             
                                                
                        if((movementDifference==6)||(movementDifference==-6)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==10)||(movementDifference==-10)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==15)||(movementDifference==-15)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==17)||(movementDifference==-17)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }  
                        return 0;            
         }
    //WQB
         
         if(movingPiece == "WQB" ){
                        //same as BQB 
                        if(pieceRow == destinationRow){
                                    return 0;            
                        }
                        
                        if(pieceColumn == destinationColumn){
                                    return 0;               
                        }
                        
                        //check column 0 and 7 cases
                        if(pieceColumn == 7){
                                       //no -7,-14,-21,-28,-35,-42,-49,-56
                                       //9,18,27,36,45,54
                                       if(movementDifference==-7){
                                                return 0;                         
                                       }
                                       if(movementDifference==-14){
                                                return 0;                         
                                       }
                                       if(movementDifference==-21){
                                                return 0;                         
                                       }
                                       if(movementDifference==-28){
                                                return 0;                         
                                       }
                                       if(movementDifference==-35){
                                                return 0;                         
                                       }
                                       if(movementDifference==-42){
                                                return 0;                         
                                       }
                                       if(movementDifference==-49){
                                                return 0;                         
                                       }
                                       if(movementDifference==-56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==9){
                                                return 0;                         
                                       }
                                       if(movementDifference==18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==54){
                                                return 0;                         
                                       }              
                        }
                        
                        if(pieceColumn == 0){
                                       //no -9,-18,-27,-36,-45,-54
                                       //7,14,21,28,35,42,49,56
                                       if(movementDifference==7){
                                                return 0;                         
                                       }
                                       if(movementDifference==14){
                                                return 0;                         
                                       }
                                       if(movementDifference==21){
                                                return 0;                         
                                       }
                                       if(movementDifference==28){
                                                return 0;                         
                                       }
                                       if(movementDifference==35){
                                                return 0;                         
                                       }
                                       if(movementDifference==42){
                                                return 0;                         
                                       }
                                       if(movementDifference==49){
                                                return 0;                         
                                       }
                                       if(movementDifference==56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==-9){
                                                return 0;                         
                                       }
                                       if(movementDifference==-18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-54){
                                                return 0;                         
                                       }
                        }      
                        
                        //general validation, movement difference is a multiple of 7 or 9
                        
                        if(movementDifference == 7){
                                              goto validMove;
                        }
                        if(movementDifference == 14){
                                              goto validMove;
                        }
                        if(movementDifference == 21){
                                              goto validMove;
                        }
                        if(movementDifference == 28){
                                              goto validMove;
                        }
                        if(movementDifference == 35){
                                              goto validMove;
                        }
                        if(movementDifference == 42){
                                              goto validMove;
                        }
                        if(movementDifference == 49){
                                              goto validMove;
                        }
                        if(movementDifference == 56){
                                              goto validMove;
                        } 
                        if(movementDifference == -7){
                                              goto validMove;
                        } 
                        if(movementDifference == -14){
                                              goto validMove;
                        }
                        if(movementDifference == -21){
                                              goto validMove;
                        }
                        if(movementDifference == -28){
                                              goto validMove;
                        }
                        if(movementDifference == -35){
                                              goto validMove;
                        }
                        if(movementDifference == -42){
                                              goto validMove;
                        }
                        if(movementDifference == -49){
                                              goto validMove;
                        }
                        if(movementDifference == -56){
                                              goto validMove;
                        }
                        if(movementDifference == 9){
                                              goto validMove;
                        } 
                        if(movementDifference == 18){
                                              goto validMove;
                        }
                        if(movementDifference == 27){
                                              goto validMove;
                        }
                        if(movementDifference == 36){
                                              goto validMove;
                        }
                        if(movementDifference == 45){
                                              goto validMove;
                        }
                        if(movementDifference == 54){
                                              goto validMove;
                        }
                        if(movementDifference == -9){
                                              goto validMove;
                        } 
                        if(movementDifference == -18){
                                              goto validMove;
                        }
                        if(movementDifference == -27){
                                              goto validMove;
                        }
                        if(movementDifference == -36){
                                              goto validMove;
                        }
                        if(movementDifference == -45){
                                              goto validMove;
                        }
                        if(movementDifference == -54){
                                              goto validMove;
                        }
                        return 0;              
         }
    //WQ
         
         if(movingPiece == "WQQ" ){
                        //same as BQ   
                        //check rook-like movement
                        if(pieceRow == destinationRow){
                                    goto validMove;            
                        }
                        if(pieceColumn == destinationColumn){
                                    goto validMove;   
                        }
                        
                        //check column 0 and 7 cases
                        if(pieceColumn == 7){
                                       //no -7,-14,-21,-28,-35,-42,-49,-56
                                       //9,18,27,36,45,54
                                       if(movementDifference==-7){
                                                return 0;                         
                                       }
                                       if(movementDifference==-14){
                                                return 0;                         
                                       }
                                       if(movementDifference==-21){
                                                return 0;                         
                                       }
                                       if(movementDifference==-28){
                                                return 0;                         
                                       }
                                       if(movementDifference==-35){
                                                return 0;                         
                                       }
                                       if(movementDifference==-42){
                                                return 0;                         
                                       }
                                       if(movementDifference==-49){
                                                return 0;                         
                                       }
                                       if(movementDifference==-56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==9){
                                                return 0;                         
                                       }
                                       if(movementDifference==18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==54){
                                                return 0;                         
                                       }              
                        }
                        
                        if(pieceColumn == 0){
                                       //no -9,-18,-27,-36,-45,-54
                                       //7,14,21,28,35,42,49,56
                                       if(movementDifference==7){
                                                return 0;                         
                                       }
                                       if(movementDifference==14){
                                                return 0;                         
                                       }
                                       if(movementDifference==21){
                                                return 0;                         
                                       }
                                       if(movementDifference==28){
                                                return 0;                         
                                       }
                                       if(movementDifference==35){
                                                return 0;                         
                                       }
                                       if(movementDifference==42){
                                                return 0;                         
                                       }
                                       if(movementDifference==49){
                                                return 0;                         
                                       }
                                       if(movementDifference==56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==-9){
                                                return 0;                         
                                       }
                                       if(movementDifference==-18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-54){
                                                return 0;                         
                                       }
                        }      
                        
                        //general validation, movement difference is a multiple of 7 or 9
                        
                        if(movementDifference == 7){
                                              goto validMove;
                        }
                        if(movementDifference == 14){
                                              goto validMove;
                        }
                        if(movementDifference == 21){
                                              goto validMove;
                        }
                        if(movementDifference == 28){
                                              goto validMove;
                        }
                        if(movementDifference == 35){
                                              goto validMove;
                        }
                        if(movementDifference == 42){
                                              goto validMove;
                        }
                        if(movementDifference == 49){
                                              goto validMove;
                        }
                        if(movementDifference == 56){
                                              goto validMove;
                        } 
                        if(movementDifference == -7){
                                              goto validMove;
                        } 
                        if(movementDifference == -14){
                                              goto validMove;
                        }
                        if(movementDifference == -21){
                                              goto validMove;
                        }
                        if(movementDifference == -28){
                                              goto validMove;
                        }
                        if(movementDifference == -35){
                                              goto validMove;
                        }
                        if(movementDifference == -42){
                                              goto validMove;
                        }
                        if(movementDifference == -49){
                                              goto validMove;
                        }
                        if(movementDifference == -56){
                                              goto validMove;
                        }
                        if(movementDifference == 9){
                                              goto validMove;
                        } 
                        if(movementDifference == 18){
                                              goto validMove;
                        }
                        if(movementDifference == 27){
                                              goto validMove;
                        }
                        if(movementDifference == 36){
                                              goto validMove;
                        }
                        if(movementDifference == 45){
                                              goto validMove;
                        }
                        if(movementDifference == 54){
                                              goto validMove;
                        }
                        if(movementDifference == -9){
                                              goto validMove;
                        } 
                        if(movementDifference == -18){
                                              goto validMove;
                        }
                        if(movementDifference == -27){
                                              goto validMove;
                        }
                        if(movementDifference == -36){
                                              goto validMove;
                        }
                        if(movementDifference == -45){
                                              goto validMove;
                        }
                        if(movementDifference == -54){
                                              goto validMove;
                        }
                        return 0;            
         }
    //WK
         
         if(movingPiece == "WKK" ){
                        
                        //vertical movement
                        if((movementDifference == 8 || movementDifference == -8) && pieceColumn == destinationColumn){
                            whiteKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == -1){
                                boardArray[pieceLocation] = "WKK";
                                boardArray[destination] = destinationSquare;
                                whiteKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            whiteKingMove = true;
                            return 1;   
                        }
                        //horizontal movement
                        if((movementDifference == 1 || movementDifference == -1) && pieceRow == destinationRow){
                            whiteKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == -1){
                                boardArray[pieceLocation] = "WKK";
                                boardArray[destination] = destinationSquare;
                                whiteKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            whiteKingMove = true;
                            return 1;
                        }

                        //diagonal movement
                        // -7
                        if((movementDifference == -7) && (pieceRow - destinationRow == 1) && (pieceColumn - destinationColumn == 1)){
                            whiteKingLoc = destination;   
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == -1){
                                boardArray[pieceLocation] = "WKK";
                                boardArray[destination] = destinationSquare;
                                whiteKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            whiteKingMove = true;
                            return 1;
                        }
                        //7
                        if((movementDifference == 7) && (pieceRow - destinationRow == -1) && (pieceColumn - destinationColumn == 1)){
                            whiteKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == -1){
                                boardArray[pieceLocation] = "WKK";
                                boardArray[destination] = destinationSquare;
                                whiteKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            whiteKingMove = true;
                            return 1;
                        }
                        //-9
                        if((movementDifference == -9) && (pieceRow - destinationRow == 1) && (pieceColumn - destinationColumn == 1)){
                            whiteKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == -1){
                                boardArray[pieceLocation] = "WKK";
                                boardArray[destination] = destinationSquare;
                                whiteKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            whiteKingMove = true;
                            return 1;
                        }
                        //9
                        if((movementDifference == 9) && (pieceRow - destinationRow == -1) && (pieceColumn - destinationColumn == -1)){
                            whiteKingLoc = destination;
                            //change board-state
                            boardArray[pieceLocation] = " E ";
                            boardArray[destination] = movingPiece;
                            if(inCheck() == -1){
                                boardArray[pieceLocation] = "WKK";
                                boardArray[destination] = destinationSquare;
                                whiteKingMove = false;
                                return 0;
                            }    
                            previousPieceLocation = pieceLocation;
                            previousDestination = destination;
                            previousMovedPiece = movingPiece;
                            whiteKingMove = true;
                            return 1;
                        }
                        return 0;               
         }
    //WKB
         
         if(movingPiece == "WKB" ){
                        //same as WQB  
                        if(pieceRow == destinationRow){
                                    return 0;            
                        }
                        
                        if(pieceColumn == destinationColumn){
                                    return 0;               
                        }
                        
                        //check column 0 and 7 cases
                        if(pieceColumn == 7){
                                       //no -7,-14,-21,-28,-35,-42,-49,-56
                                       //9,18,27,36,45,54
                                       if(movementDifference==-7){
                                                return 0;                         
                                       }
                                       if(movementDifference==-14){
                                                return 0;                         
                                       }
                                       if(movementDifference==-21){
                                                return 0;                         
                                       }
                                       if(movementDifference==-28){
                                                return 0;                         
                                       }
                                       if(movementDifference==-35){
                                                return 0;                         
                                       }
                                       if(movementDifference==-42){
                                                return 0;                         
                                       }
                                       if(movementDifference==-49){
                                                return 0;                         
                                       }
                                       if(movementDifference==-56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==9){
                                                return 0;                         
                                       }
                                       if(movementDifference==18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==54){
                                                return 0;                         
                                       }              
                        }
                        
                        if(pieceColumn == 0){
                                       //no -9,-18,-27,-36,-45,-54
                                       //7,14,21,28,35,42,49,56
                                       if(movementDifference==7){
                                                return 0;                         
                                       }
                                       if(movementDifference==14){
                                                return 0;                         
                                       }
                                       if(movementDifference==21){
                                                return 0;                         
                                       }
                                       if(movementDifference==28){
                                                return 0;                         
                                       }
                                       if(movementDifference==35){
                                                return 0;                         
                                       }
                                       if(movementDifference==42){
                                                return 0;                         
                                       }
                                       if(movementDifference==49){
                                                return 0;                         
                                       }
                                       if(movementDifference==56){
                                                return 0;                         
                                       }  
                                       if(movementDifference==-9){
                                                return 0;                         
                                       }
                                       if(movementDifference==-18){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-27){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-36){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-45){
                                                return 0;                         
                                       } 
                                       if(movementDifference==-54){
                                                return 0;                         
                                       }
                        }      
                        
                        //general validation, movement difference is a multiple of 7 or 9
                        
                        if(movementDifference == 7){
                                              goto validMove;
                        }
                        if(movementDifference == 14){
                                              goto validMove;
                        }
                        if(movementDifference == 21){
                                              goto validMove;
                        }
                        if(movementDifference == 28){
                                              goto validMove;
                        }
                        if(movementDifference == 35){
                                              goto validMove;
                        }
                        if(movementDifference == 42){
                                              goto validMove;
                        }
                        if(movementDifference == 49){
                                              goto validMove;
                        }
                        if(movementDifference == 56){
                                              goto validMove;
                        } 
                        if(movementDifference == -7){
                                              goto validMove;
                        } 
                        if(movementDifference == -14){
                                              goto validMove;
                        }
                        if(movementDifference == -21){
                                              goto validMove;
                        }
                        if(movementDifference == -28){
                                              goto validMove;
                        }
                        if(movementDifference == -35){
                                              goto validMove;
                        }
                        if(movementDifference == -42){
                                              goto validMove;
                        }
                        if(movementDifference == -49){
                                              goto validMove;
                        }
                        if(movementDifference == -56){
                                              goto validMove;
                        }
                        if(movementDifference == 9){
                                              goto validMove;
                        } 
                        if(movementDifference == 18){
                                              goto validMove;
                        }
                        if(movementDifference == 27){
                                              goto validMove;
                        }
                        if(movementDifference == 36){
                                              goto validMove;
                        }
                        if(movementDifference == 45){
                                              goto validMove;
                        }
                        if(movementDifference == 54){
                                              goto validMove;
                        }
                        if(movementDifference == -9){
                                              goto validMove;
                        } 
                        if(movementDifference == -18){
                                              goto validMove;
                        }
                        if(movementDifference == -27){
                                              goto validMove;
                        }
                        if(movementDifference == -36){
                                              goto validMove;
                        }
                        if(movementDifference == -45){
                                              goto validMove;
                        }
                        if(movementDifference == -54){
                                              goto validMove;
                        }
                        return 0;             
         }
    //WKN
         
         if(movingPiece == "WKN" ){
                        //same as WQN
                        if(pieceColumn == 0){
                                       //no 6,15,-10,-17
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }  
                                       if(movementDifference == 15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -17){
                                                             return 0;                      
                                       }          
                        }
                        
                        if(pieceColumn == 1){
                                       //no 6,-10
                                       if(movementDifference == 6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 6){
                                       //no -6,10
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                        }
                        
                        if(pieceColumn == 7){
                                       //no -6,-15,10,17
                                       if(movementDifference == -6){
                                                             return 0;                      
                                       }
                                       if(movementDifference == -15){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 10){
                                                             return 0;                      
                                       }
                                       if(movementDifference == 17){
                                                             return 0;                      
                                       }
                                       
                        }             
                                                
                        if((movementDifference==6)||(movementDifference==-6)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==10)||(movementDifference==-10)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==15)||(movementDifference==-15)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }
                        
                        if((movementDifference==17)||(movementDifference==-17)){
                                        //special cases don't exist and difference sums are valid, move is valid
                                        goto validMove;                           
                        }    
                        return 0;          
         }
    //WKR
         
         if(movingPiece == "WKR" ){
                        if(pieceRow == destinationRow){
                                    wkrMove = true;
                                    goto validMove;            
                        }
                        if(pieceColumn == destinationColumn){
                                    wkrMove = true;   
                                    goto validMove;   
                        }
                        return 0;               
         }
    //WKP

         if ((movingPiece == "WKP1") || (movingPiece == "WKP2") || (movingPiece == "WKP3") || (movingPiece == "WKP4"))
         {
                        //same as BKP except movement is -8
                        //check for crossing the board
                        if((pieceRow == 1) && (destination > -1) && (destination < 8)){
                                     //pawn has crossed the board and a new piece must be selected
                                     pawnCross = true;                                     
                        }
                        
                        
                        //check for capture
                        if(((movementDifference == -7) || (movementDifference == -9))){
                                                if((destinationColumn - pieceColumn) == 1){
                                                                     //check for passant
                                                                     if((previousPieceLocation > 7) && (previousPieceLocation < 16) && (previousDestination > 23) && (previousDestination < 32)){
                                                                         if ((previousMovedPiece == "BKP1") || (previousMovedPiece == "BQP1") || (previousMovedPiece == "BKP2") || (previousMovedPiece == "BQP2") || (previousMovedPiece == "BKP3") || (previousMovedPiece == "BQP3") || (previousMovedPiece == "BKP4") || (previousMovedPiece == "BQP4"))
                                                                         {
                                                                                                                      if((destination - previousDestination == -8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     }
                                                                     if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }    
                                                }
                                                if((destinationColumn - pieceColumn)== -1){
                                                                      //check for passant
                                                                      if((previousPieceLocation > 7) && (previousPieceLocation < 16) && (previousDestination > 23) && (previousDestination < 32)){
                                                                          if ((previousMovedPiece == "BKP1") || (previousMovedPiece == "BQP1") || (previousMovedPiece == "BKP2") || (previousMovedPiece == "BQP2") || (previousMovedPiece == "BKP3") || (previousMovedPiece == "BQP3") || (previousMovedPiece == "BKP4") || (previousMovedPiece == "BQP4"))
                                                                          {
                                                                                                                      if((destination - previousDestination == -8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     }
                                                                      if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }   
                                                }
                        }
                        //check for advancing 2 squares on first move
                        if(pieceRow == 6){
                                       if((pieceColumn == destinationColumn) && ((pieceLocation - destination == 8) || (pieceLocation - destination == 16))){
                                                       goto validMove;               
                                       }                    
                        }
                        //regular pawn movement
                        if((pieceColumn == destinationColumn) && (pieceLocation - destination == 8)){
                                        goto validMove;               
                        }               
                        return 0;
         }
    //WQP  

         if ((movingPiece == "WQP1") || (movingPiece == "WQP2") || (movingPiece == "WQP3") || (movingPiece == "WQP4"))
         {
                        //same as WKP
                        //check for crossing the board
                        if((pieceRow == 1) && (destination > -1) && (destination < 8)){
                                     //pawn has crossed the board and a new piece must be selected
                                     pawnCross = true;                                     
                        }
                        
                        
                        //check for capture
                        if(((movementDifference == -7) || (movementDifference == -9))){
                                                if((destinationColumn - pieceColumn) == 1){
                                                                      //check for passant
                                                                     if((previousPieceLocation > 7) && (previousPieceLocation < 16) && (previousDestination > 23) && (previousDestination < 32)){
                                                                         if ((previousMovedPiece == "BKP1") || (previousMovedPiece == "BQP1") || (previousMovedPiece == "BKP2") || (previousMovedPiece == "BQP2") || (previousMovedPiece == "BKP3") || (previousMovedPiece == "BQP3") || (previousMovedPiece == "BKP4") || (previousMovedPiece == "BQP4"))
                                                                         {
                                                                                                                      if((destination - previousDestination == -8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     } 
                                                                     if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }    
                                                }
                                                if((destinationColumn - pieceColumn)== -1){
                                                                      //check for passant
                                                                      if((previousPieceLocation > 7) && (previousPieceLocation < 16) && (previousDestination > 23) && (previousDestination < 32)){
                                                                          if ((previousMovedPiece == "BKP1") || (previousMovedPiece == "BQP1") || (previousMovedPiece == "BKP2") || (previousMovedPiece == "BQP2") || (previousMovedPiece == "BKP3") || (previousMovedPiece == "BQP3") || (previousMovedPiece == "BKP4") || (previousMovedPiece == "BQP4"))
                                                                          {
                                                                                                                      if((destination - previousDestination == -8) && (boardArray[destination] == " E ")){
                                                                                                                                     boardArray[previousDestination] = " E ";
                                                                                                                                     goto validMove;     
                                                                                                                      }     
                                                                                               }                               
                                                                     }
                                                                      if(destinationSquare!=" E "){
                                                                             goto validMove;                        
                                                                     }   
                                                }
                        }
                        //check for advancing 2 squares on first move
                        if(pieceRow == 6){
                                       if((pieceColumn == destinationColumn) && ((pieceLocation - destination == 8) || (pieceLocation - destination == 16))){
                                                       goto validMove;               
                                       }                    
                        }
                        //regular pawn movement
                        if((pieceColumn == destinationColumn) && (pieceLocation - destination == 8)){
                                        goto validMove;               
                        }               
                        return 0;
         }
    validMove:       
    //check for piece capture boardArray[destination]!="E" add captured piece to players capture list(not implemented)
    
    
    //store current move for previous in next move call
    previousPieceLocation = pieceLocation;
    previousDestination = destination;
    previousMovedPiece = movingPiece;
    //change board-state
    boardArray[pieceLocation] = " E ";
    boardArray[destination] = movingPiece;    

    //if a pawn has crossed the board return -1
    if(pawnCross){
                  //a call to pawnReplace must be made when -1 is returned or pawnCross with remain true!!!
                  return -1;     
    }
    //if fell through function: return from function with value of 1
    return 1;     
}


    }
}
