import cv2
import os
import re
from tkinter import Label, Tk
from board import Board
from piece import Piece
from PIL import ImageTk, Image, ImageOps


class Player(object):

    def __init__(self, color="black", king=None):
        self.color = color
        self.in_check = False


class Game(object):

    def __init__(self):
        self.board = Board()
        self.player1 = Player(color="white")
        self.player2 = Player()
        self.turn = self.player1

    def all_squares(self):
        squares = []
        for y in range(1,9):
            for x in ["a", "b", "c", "d", "e", "f", "g", "h"]:
                squares.append("%s%s" % (x, y))
        
        return squares

    def try_move(self, square):
        black_king_square = self.board.black_king
        white_king_square = self.board.white_king
        source = self.board.text_to_square(square)

        try:
            if source.piece.color is not "black" and source.piece.name() is not ' ':
                black_valid = self.move(square, black_king_square, checking_for_check=True)
                if black_valid:
                    print("Black in check from %s" % square)
                    self.player2.in_check = True
                    self.board.text_to_square(black_king_square).piece.in_check = True
                    return True

        except AssertionError:
            pass

        try:
            if source.piece.color is not "white" and source.piece.name() is not ' ':
                white_valid = self.move(square, white_king_square, checking_for_check=True)
                if white_valid:
                    print("White in check from %s" % square)
                    self.player1.in_check = True
                    self.board.text_to_square(white_king_square).piece.in_check = True
                    return True

        except AssertionError:
            pass

        return False


    def check_for_check(self):
        self.board.text_to_square(self.board.black_king).piece.in_check = False
        self.board.text_to_square(self.board.white_king).piece.in_check = False
        self.player1.in_check = False
        self.player2.in_check = False
        for square in self.all_squares():
            self.try_move(square)

        print("Black in Check:%s White in Check:%s" % (self.player2.in_check, self.player1.in_check))

    def move(self, source_square, destination_square, checking_for_check=False):
        assert source_square != destination_square
        assert re.match('[a-h]{1}[1-8]{1}', destination_square)
        assert re.match('[a-h]{1}[1-8]{1}', source_square)
        source = self.board.text_to_square(source_square)
        destination = self.board.text_to_square(destination_square)
        piece_name = source.piece.name()
        piece_color = source.piece.color
        if not checking_for_check:
            assert piece_color == self.turn.color

        assert piece_color != destination.piece.color
        assert source.piece.valid_move(source_square, destination_square, self.board, self, checking_for_check)
        path = self.board.path(source_square, destination_square)
        print("%s %s\n%s\n%s" % (source_square, destination_square, self.board, map(lambda x: x.coords, path)))
        print("Black king: %s White King: %s" % (self.board.black_king, self.board.white_king))
        print("%s" % piece_name)

        if piece_name != "knight" and len(path) > 2 and piece_name != "king":
            for square in path[1:-1]:
                assert square.piece.name() == ' '

        if checking_for_check:
            return True
        else:
            dest_bkp_piece = destination.piece
            source_bkp_piece = source.piece
            destination.piece = source.piece
            source.piece = Piece()
            self.check_for_check()
            if self.turn.in_check:
                destination.piece = dest_bkp_piece
                source.piece = source_bkp_piece
                self.check_for_check()
                return False

        self.board.last_move = destination

        if piece_name == "king":
            if piece_color == "black":
                self.board.black_king = destination_square
            elif piece_color == "white":
                self.board.white_king = destination_square

        if self.turn.color == "white":
            self.turn = self.player2
        elif self.turn.color == "black":
            self.turn = self.player1

        if source_square == "a1":
            self.board.a1_has_moved = True
        elif source_square == "a8":
            self.board.a8_has_moved = True
        elif source_square == "e1":
            self.board.e1_has_moved = True
        elif source_square == "e8":
            self.board.e8_has_moved = True
        elif source_square == "h1":
            self.board.h1_has_moved = True
        elif source_square == "h8":
            self.board.h8_has_moved = True

        print("It's %s's turn" % self.turn.color)
        return


class GameLoop(object):

    def get_frame(self):
        board_path = "chess.jpg"
        img = Image.open(board_path)
        for coords in self.game.all_squares():
            square = self.game.board.text_to_square(coords)
            image = square.piece.image
            point = self.game.board.convert_square_to_point(square)
            if image is not None:
                img.paste(image, point, image)

        return ImageTk.PhotoImage(img)

    def click_event(self, event):
        self.window.title("Chess")
        if self.first_click:
            self.first_click_x = event.x
            self.first_click_y = event.y
            try:
                square = self.game.board.convert_point_to_square(self.first_click_x, self.first_click_y) 
                square.piece.image = ImageOps.crop(square.piece.image, border=3)
                square.piece.image = ImageOps.expand(square.piece.image, border=3, fill='indianred')
            except AttributeError:
                square.piece.image = square.piece.get_image()
                self.window.title("Chess")

            self.first_click = False
            frame = self.get_frame()
            self.panel.configure(image = frame)
            self.panel.image = frame
        else:
            source = self.game.board.convert_point_to_square(self.first_click_x, self.first_click_y)
            destination = self.game.board.convert_point_to_square(event.x, event.y)
            try:
                self.game.move(source.coords, destination.coords)
                print("%s %s" % (source.coords, destination.coords))
            except AssertionError:
                self.window.title("Chess - Invalid Move")
                pass
            except AttributeError:
                pass
            finally:
                frame = self.get_frame()
                self.panel.configure(image = frame)
                self.panel.image = frame
                source.piece.image = source.piece.get_image()
                destination.piece.image = destination.piece.get_image()
                self.first_click = True

    def __init__(self):
        os.system('clear')
        self.window = Tk()
        self.game = Game()
        self.first_click = True
        self.first_click_x = None
        self.first_click_y = None
        self.window.title("Chess")
        self.window.geometry("768x768")
        self.window.configure(background='grey')
        frame = self.get_frame()
        self.panel = Label(self.window, image = frame)
        self.panel.bind('<Button-1>', self.click_event)
        self.panel.pack(side = "bottom", fill = "both", expand = "yes")
        self.window.mainloop()

if __name__ == "__main__":
    GameLoop()
