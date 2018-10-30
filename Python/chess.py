import argparse
import cv2
import json
import logging
import os
import re
import time
from tkinter import Label, Tk
from board import Board
from piece import Piece
from chess_exceptions import InvalidMoveException
from PIL import ImageTk, Image, ImageOps

logging.basicConfig(filename='chess.log', level=logging.INFO)

click_map = None
with open('click_map.json') as data:
    click_map = json.load(data)


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
        self.move_counter = 0

    def all_squares(self):
        squares = []
        for y in range(1,9):
            for x in ["a", "b", "c", "d", "e", "f", "g", "h"]:
                squares.append("%s%s" % (x, y))
        
        return squares

    def update_threats(self):
        self.clear_threats()
        for source in self.all_squares():
            for destination in self.all_squares():
                try:
                    self.move(source, destination, updating_threats=True)
                    source_piece = self.board.text_to_square(source).piece
                    destination_piece = self.board.text_to_square(destination).piece
                    logging.info("{} {} {} threatens {} {} {}".format(source, 
                        source_piece.color,
                        source_piece.name(),
                        destination,
                        destination_piece.color,
                        destination_piece.name()))

                    if destination == self.board.white_king and source_piece.color != "white":
                        white_king = self.board.text_to_square(destination).piece.in_check = True
                        self.player1.in_check = True

                    if destination == self.board.black_king and source_piece.color != "black":
                        black_king = self.board.text_to_square(destination).piece.in_check = True
                        self.player2.in_check = True

                except InvalidMoveException:
                    pass


    def clear_threats(self):
        self.board.text_to_square(self.board.white_king).piece.in_check = False
        self.board.text_to_square(self.board.black_king).piece.in_check = False
        self.player1.in_check = False
        self.player2.in_check = False
        for square in self.all_squares():
            square = self.board.text_to_square(square)
            square.threats = dict()

    def move(self, source_square, destination_square, updating_threats=False):
        if source_square == destination_square:
            raise InvalidMoveException("Source square is equal to Destination square")

        if not re.match('[a-h]{1}[1-8]{1}', destination_square):
            raise InvalidMoveException("Invalid Destination square")

        if not re.match('[a-h]{1}[1-8]{1}', source_square):
            raise InvalidMoveException("Invalid Source square")

        source = self.board.text_to_square(source_square)
        destination = self.board.text_to_square(destination_square)
        piece_name = source.piece.name()
        piece_color = source.piece.color
        if not updating_threats:
            if piece_color != self.turn.color:
                raise InvalidMoveException("Piece color does not match turn color")

        if piece_color == destination.piece.color:
            raise InvalidMoveException("Piece color cannot match destination piece color")

        if not source.piece.valid_move(source_square, destination_square, self.board, self, updating_threats):
            raise InvalidMoveException("Invalid move for piece")

        path = self.board.path(source_square, destination_square)
        #logging.info("%s %s\n%s\n%s" % (source_square, destination_square, self.board, map(lambda x: x.coords, path)))
        logging.info("Black king: %s White King: %s" % (self.board.black_king, self.board.white_king))
        logging.info("Source piece is: %s" % piece_name)

        if piece_name != "knight" and len(path) > 2 and piece_name != "king":
            for square in path[1:-1]:
                if square.piece.name() != ' ':
                    raise InvalidMoveException("Square is not empty")

        if updating_threats:
            self.board.text_to_square(destination_square).threats[source_square] = self.move_counter
            return True
        else:
            dest_bkp_piece = destination.piece
            source_bkp_piece = source.piece
            destination.piece = source.piece
            source.piece = Piece()
            self.update_threats()
            if self.turn.in_check:
                destination.piece = dest_bkp_piece
                source.piece = source_bkp_piece
                self.update_threats()
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

        self.move_counter += 1
        self.update_threats()


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
            logging.info("Click location x:{} - y:{}".format(event.x, event.y))
            square = self.game.board.convert_point_to_square(self.first_click_x, self.first_click_y) 
            if square.piece.image is not None:
                square.piece.image = ImageOps.crop(square.piece.image, border=3)
                square.piece.image = ImageOps.expand(square.piece.image, border=3, fill='lightgreen')

            self.first_click = False
            frame = self.get_frame()
            self.panel.configure(image = frame)
            self.panel.image = frame
        else:
            source = self.game.board.convert_point_to_square(self.first_click_x, self.first_click_y)
            destination = self.game.board.convert_point_to_square(event.x, event.y)
            try:
                self.game.move(source.coords, destination.coords)
            except InvalidMoveException as e:
                logging.info(e.message)
                self.window.title("Chess - Invalid Move")
            finally:
                frame = self.get_frame()
                self.panel.configure(image = frame)
                self.panel.image = frame
                source.piece.image = source.piece.get_image()
                destination.piece.image = destination.piece.get_image()
                self.first_click = True


    def move_script_event(self, event):
        logging.info("Attempting to execute move script")
        with open(self.movelistfile) as moves:
            line = moves.readline()
            while line:
                source, destination = line.replace('\n', '').split(":")
                source_x = int(click_map[source]['x'])
                source_y = int(click_map[source]['y'])
                dest_x = int(click_map[destination]['x'])
                dest_y = int(click_map[destination]['y'])
                logging.info("Executing scripted move")
                event.x = source_x
                event.y = source_y
                self.click_event(event)
                self.window.update()
                event.x = dest_x
                event.y = dest_y
                self.click_event(event)
                time.sleep(1)
                line = moves.readline()


    def __init__(self, movelistfile):
        os.system('clear')
        self.window = Tk()
        self.game = Game()
        self.movelistfile = movelistfile
        self.first_click = True
        self.first_click_x = None
        self.first_click_y = None
        self.window.title("Chess")
        self.window.geometry("768x768")
        self.window.configure(background='grey')
        frame = self.get_frame()
        self.panel = Label(self.window, image = frame)
        self.panel.bind('<Button-1>', self.click_event)
        self.panel.bind('<Button-3>', self.move_script_event)
        self.panel.pack(side = "bottom", fill = "both", expand = "yes")
        self.window.mainloop()


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--movelistfile')
    args = parser.parse_args()
    GameLoop(args.movelistfile)
