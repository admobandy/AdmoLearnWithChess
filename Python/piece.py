import logging
from PIL import Image
from chess_exceptions import InvalidMoveException


class Piece(object):

    def __init__(self, color=' '):
        self.color = color
        self.image = self.get_image()

    def get_image(self):
        if self.color == ' ' or self.name == ' ':
            return
        else:
            return Image.open("%s_%s.png" % (self.color, self.name()))

    def name(self):
        return ' ' 

    def valid_move(self, s, d, board, game, updating_threats):
        return False

    def is_single_square_move(self, s_x, s_y, d_x, d_y, board):
        x_diff = d_x - s_x
        y_diff = d_y - s_y

        if x_diff > 1 or x_diff < -1:
            return False

        if y_diff > 1 or y_diff < -1:
            return False

        return True

    def is_diagonal_move(self, s_x, s_y, d_x, d_y, board):
        x_diff = d_x - s_x
        y_diff = d_y - s_y

        if x_diff == 0 or y_diff == 0:
            return False

        if x_diff < 0:
            x_diff = x_diff * -1

        if y_diff < 0:
            y_diff = y_diff * -1

        if x_diff == y_diff:
            return True

        return False

    def is_rank_move(self, x_diff, y_diff):
        if x_diff != 0 and y_diff == 0:
            return True

        return False

    def is_file_move(self, x_diff, y_diff):
        if y_diff != 0 and x_diff == 0:
            return True

        return False

    def is_rank_or_file_move(self, s_x, s_y, d_x, d_y, board):
        x_diff = d_x - s_x
        y_diff = d_y - s_y

        if self.is_rank_move(x_diff, y_diff):
            return True

        if self.is_file_move(x_diff, y_diff):
            return True

        return False

    def is_moving_forward(self, s_x, s_y, d_x, d_y, board):
        x_diff = d_x - s_x
        y_diff = d_y - s_y
        if self.color == "white" and x_diff == 0 and y_diff > 0:
            return True

        if self.color == "black" and x_diff == 0 and y_diff < 0:
            return True

        return False

    def is_l_move(self, s_x, s_y, d_x, d_y, board):
        x_diff = d_x - s_x
        y_diff = d_y - s_y

        if x_diff == 2 or x_diff == -2:
            if y_diff == 1 or y_diff == -1:
                return True

        if y_diff == 2 or y_diff == -2:
            if x_diff == 1 or x_diff == -1:
                return True

        return False

    def destination_is_empty(self, d_x, d_y, board):
        logging.debug("Checking if destination is empty for {} {}".format(d_x, d_y))
        location = "%s%s" % (board.convert_x_axis_to_letter(d_x), d_y)
        location = board.text_to_square(location)
        if location.piece.name() == ' ':
            return True

        return False

    def is_en_passant(self, s_x, s_y, d_x, d_y, board, updating_threats):
        if self.color == "white" and s_y != 5:
            return False

        if self.color == "black" and s_y != 4:
            return False

        y_diff = d_y - s_y
        diagonal = self.is_diagonal_move(s_x, s_y, d_x, d_y, board)
        logging.debug("Checking pawn en passant {} {}".format(d_x, d_y))
        capture = "%s%s" % (board.convert_x_axis_to_letter(d_x), d_y - y_diff)
        capture = board.text_to_square(capture)
        if diagonal and self.destination_is_empty(d_x, d_y, board) and capture.piece.name() == "pawn":
            if board.last_move == capture and not updating_threats:
                capture.piece = Piece()
                return True

        return False

    def is_castle(self, s_x, s_y, d_x, d_y, board, game):
        logging.info("Checking for castle")
        source = "%s%s" % (board.convert_x_axis_to_letter(s_x), s_y)
        destination = "%s%s" % (board.convert_x_axis_to_letter(d_x), d_y)
        source_square = board.text_to_square(source)
        destination_square = board.text_to_square(destination)
        
        path = board.path(source, destination)
        
        if len(path) < 2:
            return False

        for square in path[1:]:
            if square.piece.name() != ' ':
                return False

        if source == "e1" and self.color == "white":
            if destination == "g1":
                if board.e1_has_moved or board.h1_has_moved:
                    raise InvalidMoveException("King or Rook has already moved")

                for coords in board.f1.threats.keys():
                    if board.text_to_square(coords).piece.color == "black":
                        return False
                for coords in board.g1.threats.keys():
                    if board.text_to_square(coords).piece.color == "black":
                        return False

                game.move("h1", "f1", keep_turn=True)
                return True

            elif destination == "c1" and board.b1.piece.name() == ' ':
                if board.e1_has_moved or board.a1_has_moved:
                    raise InvalidMoveException("King or Rook has already moved")

                for coords in board.b1.threats.keys():
                    if board.text_to_square(coords).piece.color == "black":
                        return False
                for coords in board.c1.threats.keys():
                    if board.text_to_square(coords).piece.color == "black":
                        return False
                for coords in board.d1.threats.keys():
                    if board.text_to_square(coords).piece.color == "black":
                        return False

                game.move("a1", "d1", keep_turn=True)
                return True

        if source == "e8" and self.color == "black":
            if destination == "g8":
                if board.e8_has_moved or board.h8_has_moved:
                    raise InvalidMoveException("King or Rook has already moved")

                for coords in board.f8.threats.keys():
                    if board.text_to_square(coords).piece.color == "white":
                        return False
                for coords in board.g8.threats.keys():
                    if board.text_to_square(coords).piece.color == "white":
                        return False

                game.move("h8", "f8", keep_turn=True)
                return True

            elif destination == "c8" and board.b8.piece.name() == ' ':
                if board.e8_has_moved or board.a8_has_moved:
                    raise InvalidMoveException("King or Rook has already moved")

                for coords in board.b8.threats.keys():
                    if board.text_to_square(coords).piece.color == "white":
                        return False
                for coords in board.c8.threats.keys():
                    if board.text_to_square(coords).piece.color == "white":
                        return False
                for coords in board.d8.threats.keys():
                    if board.text_to_square(coords).piece.color == "white":
                        return False

                game.move("a8", "d8", keep_turn=True)
                return True

        return False

    def is_pawn_upgrade(self, s, d, board):
        s_y = s[1]
        d_y = d[1]
        if s_y == '7' and d_y == '8' and self.color == 'white':
            return True

        if s_y == '2' and d_y == '1' and self.color == 'black':
            return True

        return False

class Pawn(Piece):

    def __init__(self, color):
        super(Pawn, self).__init__(color=color)

    def first_move(self, s_y):
        if self.color == 'white' and s_y == 2:
            return True
        elif self.color == 'black' and s_y == 7:
            return True
        else:
            return False

    def name(self):
        return "pawn"

    def valid_move(self, s, d, board, game, updating_threats):
        valid = self._valid_move(s, d, board, game, updating_threats)
        logging.debug("pawn debug, move is valid: %s", valid)
        if not updating_threats:
            if self.is_pawn_upgrade(s, d, board):
                # TODO: Handle piece upgrade with more than auto-queen
                square = board.text_to_square(s)
                square.piece = Queen(self.color)
                logging.info("Piece swap")

        return valid

    def _valid_move(self, s, d, board, game, updating_threats):
        s_x, s_y, d_x, d_y = board.get_coords(s, d)
        x_diff = d_x - s_x
        y_diff = d_y - s_y
        if self.color == 'white' and y_diff < 0:
            return False

        if self.color == 'black' and y_diff > 0:
            return False

        if not self.first_move(s_y):
            if y_diff > 1 and self.color == 'white':
                return False
        
            if y_diff < -1 and self.color == 'black':
                return False

        # are we moving straight ahead
        if self.is_moving_forward(s_x, s_y, d_x, d_y, board):
            if self.first_move(s_y):
                if y_diff > 2 and self.color == 'white':
                    return False

                if y_diff < -2 and self.color == 'black':
                    return False
                
            if board.text_to_square(d).piece.name() == ' ':
                return True
            else:
                logging.debug("pawn debug, illegal move, forward square is not empty ")
                return False

        # are we capturing diagonally
        if self.is_diagonal_move(s_x, s_y, d_x, d_y, board) and updating_threats:
            if y_diff == 1 and self.color == 'white':
                return True

            if y_diff == -1 and self.color == 'black':
                return True

        if self.is_diagonal_move(s_x, s_y, d_x, d_y, board) and not self.destination_is_empty(d_x, d_y, board):
            if self.first_move(s_y):
                if y_diff >= 2 and self.color == 'white':
                    return False

                if y_diff <= -2 and self.color == 'black':
                    return False

            return True

        # is this an en passant
        if self.is_en_passant(s_x, s_y, d_x, d_y, board, updating_threats):
            return True

        logging.debug("pawn debug, illegal move, We've fallen all the way through our checks")  
        return False

class Rook(Piece):

    def __init__(self, color):
        super(Rook, self).__init__(color=color)

    def name(self):
        return "rook"

    def valid_move(self, s, d, board, game, updating_threats):
        s_x, s_y, d_x, d_y = board.get_coords(s, d)
        # are we not moving diagonally
        return self.is_rank_or_file_move(s_x, s_y, d_x, d_y, board)

class Knight(Piece):

    def __init__(self, color):
        super(Knight, self).__init__(color=color)

    def name(self):
        return "knight"

    def valid_move(self, s, d, board, game, updating_threats):
        s_x, s_y, d_x, d_y = board.get_coords(s, d)
        return self.is_l_move(s_x, s_y, d_x, d_y, board)

class Bishop(Piece):

    def __init__(self, color):
        super(Bishop, self).__init__(color=color)

    def name(self):
        return "bishop"

    def valid_move(self, s, d, board, game, updating_threats):
        s_x, s_y, d_x, d_y = board.get_coords(s, d)
        if self.is_diagonal_move(s_x, s_y, d_x, d_y, board):
            return True

        return False

class Queen(Piece):

    def __init__(self, color):
        super(Queen, self).__init__(color=color)

    def name(self):
        return "queen"

    def valid_move(self, s, d, board, game, updating_threats):
        s_x, s_y, d_x, d_y = board.get_coords(s, d)
        # are we moving diagonally
        diagonal = self.is_diagonal_move(s_x, s_y, d_x, d_y, board)
        # are we moving a single square
        single = self.is_single_square_move(s_x, s_y, d_x, d_y, board)
        # are we moving along a rank or file
        rank_or_file = self.is_rank_or_file_move(s_x, s_y, d_x, d_y, board)
        if diagonal or single or rank_or_file:
            return True

        return False

class King(Piece):

    def __init__(self, color):
       self.in_check = False
       super(King, self).__init__(color=color)

    def name(self):
        return "king"

    def valid_move(self, s, d, board, game, updating_threats):
        s_x, s_y, d_x, d_y = board.get_coords(s, d)
        source = board.text_to_square(s)
        destination = board.text_to_square(d)
        # are we moving a single square
        single = self.is_single_square_move(s_x, s_y, d_x, d_y, board)

        if single:
            if self.color == 'black' and not updating_threats:
                for coords in destination.threats.keys():
                    if board.text_to_square(coords).piece.color == "white": 
                        return False
            elif self.color == 'white' and not updating_threats:
                for coords in destination.threats.keys():
                    if board.text_to_square(coords).piece.color == "black":
                        return False

            return True
        # are we castling
        castle = False
        if not self.in_check and not updating_threats:
            return self.is_castle(s_x, s_y, d_x, d_y, board, game)

        return False