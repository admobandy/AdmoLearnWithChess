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
from chess_exceptions import InvalidMoveException, InvalidArgumentsException
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
        self.white_material_score = 0
        self.black_material_score = 0
        self.white_position_score = 0
        self.black_position_score = 0

    def all_squares(self):
        squares = []
        for y in range(1,9):
            for x in ["a", "b", "c", "d", "e", "f", "g", "h"]:
                squares.append("%s%s" % (x, y))
        
        return squares

    def get_piece_value(self, piece):
        if piece.name() == 'pawn':
            return 1
        elif piece.name() == 'rook':
            return 5
        elif piece.name() == 'knight':
            return 3
        elif piece.name() == 'bishop':
            return 3
        elif piece.name() == 'queen':
            return 8
        elif piece.name() == 'king':
            return 10
        else:
            return 0

    def update_threats(self):
        self.clear_threats()
        self.white_material_score = 0
        self.black_material_score = 0
        self.white_position_score = 0
        self.black_position_score = 0
        # Create threat matrix
        # Evaluate material scores
        for source in self.all_squares():
            source_piece = self.board.text_to_square(source).piece
            if source_piece.color == 'white':
                self.white_material_score += self.get_piece_value(source_piece)
            elif source_piece.color == 'black':
                self.black_material_score += self.get_piece_value(source_piece)

        # Evalute position scores
        for source in self.all_squares():
            for destination in self.all_squares():
                try:
                    self.move(source, destination, updating_threats=True)
                    source_piece = self.board.text_to_square(source).piece
                    destination_piece = self.board.text_to_square(destination).piece
                       
                    if destination == self.board.white_king and source_piece.color != "white":
                        white_king = self.board.text_to_square(destination).piece.in_check = True
                        self.player1.in_check = True

                    if destination == self.board.black_king and source_piece.color != "black":
                        black_king = self.board.text_to_square(destination).piece.in_check = True
                        self.player2.in_check = True
                    
                    if source_piece.color == 'white':
                        self.white_position_score += self.get_piece_value(source_piece) 
                    elif source_piece.color == 'black':
                        self.black_position_score += self.get_piece_value(source_piece)

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

    def move(self, source_square, destination_square, updating_threats=False, keep_turn=False):
        player1_already_in_check = self.player1.in_check
        player2_already_in_check = self.player2.in_check

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
        if not updating_threats:
            logging.info("%s %s\n%s\n%s" % (source_square, destination_square, self.board, map(lambda x: x.coords, path)))
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
            if destination.piece.name() == "king":
                if destination.piece.color == "white":
                    self.board.white_king = destination_square
                if destination.piece.color == "black":
                    self.board.black_king = destination_square

            self.update_threats()

            if self.turn.in_check:
                if destination.piece.name() == "king":
                    if destination.piece.color == "white":
                        self.board.white_king = source_square
                    if destination_piece.color == "black":
                        self.board.black_king = source_square

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

        if not keep_turn:
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
                if self.game.player1.in_check or self.game.player2.in_check:
                    self.window.title("Chess - Check")
                
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
                time.sleep(float(self.movelistsleep))
                line = moves.readline()


    def __init__(self, movelistfile, movelistsleep, white_ai=False, black_ai=False):
        os.system('clear')
        self.white_ai = white_ai
        self.black_ai = black_ai
        self.window = Tk()
        self.game = Game()
        self.movelistfile = movelistfile
        self.movelistsleep = movelistsleep
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
        if self.white_ai or self.black_ai:
            self.window.after(500, self.pump_ai_event)

        self.window.mainloop()

    def pump_ai_event(self):
        self.window.title("Chess - AI Processing")
        self.window.update()
        if self.black_ai and self.game.turn.color == 'black':
            self.evaluate_ai_moves('black')
        elif self.white_ai and self.game.turn.color == 'white':
            self.evaluate_ai_moves('white')
        else:
            pass
        self.window.after(100, self.pump_ai_event)

    def evaluate_ai_moves(self, color):
        logging.info("Starting AI move generation")
        # Get a list of valid moves for this color
        self.game.update_threats()
        pieces = []
        opponent_pieces = []
        possible_moves = dict()
        opponent_moves = dict()
        logging.info("Gathering pieces for each color")
        for location in self.game.all_squares():
            current_square = self.game.board.text_to_square(location)
            if current_square.piece.color == color:
                pieces.append(location)
            elif current_square.piece.color != ' ':
                opponent_pieces.append(location)

        logging.info("Gathering available moves for each color")
        for location in self.game.all_squares():
            current_square = self.game.board.text_to_square(location)
            for piece in pieces:
                if piece in current_square.threats.keys():
                    possible_moves[(piece, location)] = 0
            for opponent_piece in opponent_pieces:
                if opponent_piece in current_square.threats.keys():
                    opponent_moves[(opponent_piece, location)] = 0

        logging.info("Evaluating material") 
        for source, destination in possible_moves:
            source_value = self.game.get_piece_value(self.game.board.text_to_square(source).piece)
            dest_value = self.game.get_piece_value(self.game.board.text_to_square(destination).piece)
            diff = dest_value - source_value
            if diff > 0 and dest_value > 0:
                logging.info("AI taking higher value piece")
                self.perform_ai_move(source, destination)
                return
            elif diff == 0 and dest_value > 0:
                logging.info("AI Performing material exchange")
                self.perform_ai_move(source, destination)
                return

            possible_moves[(source, destination)] = diff

        for source, destination in opponent_moves:
            source_value = self.game.get_piece_value(self.game.board.text_to_square(source).piece)
            dest_value = self.game.get_piece_value(self.game.board.text_to_square(destination).piece)
            diff = dest_value - source_value
            opponent_moves[(source, destination)] = diff

        # Evaluate the existing threat matrix
        # Am I in check?
        if color == 'black' and self.game.player2.in_check:
            # I'm in check!!!
            logging.info("%s AI player is in check", color)
        elif color == 'white' and self.game.player1.in_check:
            # I'm in check!!!
            logging.info("%s AI player is in check", color)

        # Are any of my pieces threatened?
        # pump ML model
        # generate move
        # if all else fails just select the first available
        global move_index
        move_index = 0
        for key in possible_moves.keys():
           try:
               logging.info("Move index: %s", move_index)
               source, destination = possible_moves.keys()[move_index]
               self.perform_ai_move(source, destination)
               move_index = 0
           except InvalidMoveException as e:
               logging.error("AI attempted invalid move: %s", e.message)
               move_index += 1
               continue

    def perform_ai_move(self, source, destination):
        self.click_event(Event(click_map[source]['x'], click_map[source]['y']))
        self.click_event(Event(click_map[destination]['x'], click_map[destination]['y']))
        self.window.title("Chess")


class Event(object):

    def __init__(self, x, y):
        self.x = x
        self.y = y


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--movelistfile')
    parser.add_argument('--movelistsleep', default=1)
    parser.add_argument('--white_ai', action='store_true', default=False)
    parser.add_argument('--black_ai', action='store_true', default=False)
    args = parser.parse_args()
    if args.movelistfile:
        if args.white_ai or args.black_ai:
            raise InvalidArgumentsException("Cannot use move list file with AI")

    GameLoop(args.movelistfile, args.movelistsleep, white_ai=args.white_ai, black_ai=args.black_ai)
