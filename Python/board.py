import logging
from piece import Pawn, Rook, Knight, Bishop, Queen, King, Piece
from chess_exceptions import InvalidMoveException

class Square(object):
   
    def __init__(self, coords, piece=None):
        self.piece = piece
        self.coords = coords
        self.threats = dict()

    def __str__(self):
        return "%s%s" % (self.piece.color[0], self.piece.name()[0])

class Board(object):

    def __init__(self):
        self.last_move = None
        self.a1=Square("a1", piece=Rook(color="white"))
        self.b1=Square("b1", piece=Knight(color="white"))
        self.c1=Square("c1", piece=Bishop(color="white"))
        self.d1=Square("d1", piece=Queen(color="white"))
        self.e1=Square("e1", piece=King(color="white"))
        self.f1=Square("f1", piece=Bishop(color="white"))
        self.g1=Square("g1", piece=Knight(color="white"))
        self.h1=Square("h1", piece=Rook(color="white"))
        self.a2=Square("a2", piece=Pawn(color="white"))
        self.b2=Square("b2", piece=Pawn(color="white"))
        self.c2=Square("c2", piece=Pawn(color="white"))
        self.d2=Square("d2", piece=Pawn(color="white"))
        self.e2=Square("e2", piece=Pawn(color="white"))
        self.f2=Square("f2", piece=Pawn(color="white"))
        self.g2=Square("g2", piece=Pawn(color="white"))
        self.h2=Square("h2", piece=Pawn(color="white"))
        self.a3=Square("a3", piece=Piece())
        self.b3=Square("b3", piece=Piece())
        self.c3=Square("c3", piece=Piece())
        self.d3=Square("d3", piece=Piece())
        self.e3=Square("e3", piece=Piece())
        self.f3=Square("f3", piece=Piece())
        self.g3=Square("g3", piece=Piece())
        self.h3=Square("h3", piece=Piece())
        self.a4=Square("a4", piece=Piece())
        self.b4=Square("b4", piece=Piece())
        self.c4=Square("c4", piece=Piece())
        self.d4=Square("d4", piece=Piece())
        self.e4=Square("e4", piece=Piece())
        self.f4=Square("f4", piece=Piece())
        self.g4=Square("g4", piece=Piece())
        self.h4=Square("h4", piece=Piece())
        self.a5=Square("a5", piece=Piece())
        self.b5=Square("b5", piece=Piece())
        self.c5=Square("c5", piece=Piece())
        self.d5=Square("d5", piece=Piece())
        self.e5=Square("e5", piece=Piece())
        self.f5=Square("f5", piece=Piece())
        self.g5=Square("g5", piece=Piece())
        self.h5=Square("h5", piece=Piece())
        self.a6=Square("a6", piece=Piece())
        self.b6=Square("b6", piece=Piece())
        self.c6=Square("c6", piece=Piece())
        self.d6=Square("d6", piece=Piece())
        self.e6=Square("e6", piece=Piece())
        self.f6=Square("f6", piece=Piece())
        self.g6=Square("g6", piece=Piece())
        self.h6=Square("h6", piece=Piece())
        self.a7=Square("a7", piece=Pawn(color="black"))
        self.b7=Square("b7", piece=Pawn(color="black"))
        self.c7=Square("c7", piece=Pawn(color="black"))
        self.d7=Square("d7", piece=Pawn(color="black"))
        self.e7=Square("e7", piece=Pawn(color="black"))
        self.f7=Square("f7", piece=Pawn(color="black"))
        self.g7=Square("g7", piece=Pawn(color="black"))
        self.h7=Square("h7", piece=Pawn(color="black"))
        self.a8=Square("a8", piece=Rook(color="black"))
        self.b8=Square("b8", piece=Knight(color="black"))
        self.c8=Square("c8", piece=Bishop(color="black"))
        self.d8=Square("d8", piece=Queen(color="black"))
        self.e8=Square("e8", piece=King(color="black"))
        self.f8=Square("f8", piece=Bishop(color="black"))
        self.g8=Square("g8", piece=Knight(color="black"))
        self.h8=Square("h8", piece=Rook(color="black"))
        self.white_king = "e1"
        self.black_king = "e8"
        self.a1_has_moved = False
        self.a8_has_moved = False
        self.e1_has_moved = False
        self.e8_has_moved = False
        self.h1_has_moved = False
        self.h8_has_moved = False

    def text_to_square(self, text):
        ttsd = {"a1": self.a1,
                "b1": self.b1,
                "c1": self.c1,
                "d1": self.d1,
                "e1": self.e1,
                "f1": self.f1,
                "g1": self.g1,
                "h1": self.h1,
                "a2": self.a2,
                "b2": self.b2,
                "c2": self.c2,
                "d2": self.d2,
                "e2": self.e2,
                "f2": self.f2,
                "g2": self.g2,
                "h2": self.h2,
                "a3": self.a3,
                "b3": self.b3,
                "c3": self.c3,
                "d3": self.d3,
                "e3": self.e3,
                "f3": self.f3,
                "g3": self.g3,
                "h3": self.h3,
                "a4": self.a4,
                "b4": self.b4,
                "c4": self.c4,
                "d4": self.d4,
                "e4": self.e4,
                "f4": self.f4,
                "g4": self.g4,
                "h4": self.h4,
                "a5": self.a5,
                "b5": self.b5,
                "c5": self.c5,
                "d5": self.d5,
                "e5": self.e5,
                "f5": self.f5,
                "g5": self.g5,
                "h5": self.h5,
                "a6": self.a6,
                "b6": self.b6,
                "c6": self.c6,
                "d6": self.d6,
                "e6": self.e6,
                "f6": self.f6,
                "g6": self.g6,
                "h6": self.h6,
                "a7": self.a7,
                "b7": self.b7,
                "c7": self.c7,
                "d7": self.d7,
                "e7": self.e7,
                "f7": self.f7,
                "g7": self.g7,
                "h7": self.h7,
                "a8": self.a8,
                "b8": self.b8,
                "c8": self.c8,
                "d8": self.d8,
                "e8": self.e8,
                "f8": self.f8,
                "g8": self.g8,
                "h8": self.h8,}
        try:
            return ttsd[text]
        except KeyError:
            pass

    def y_transform(self, y):
        transform = {'1': '8',
                     '2': '7',
                     '3': '6',
                     '4': '5',
                     '5': '4',
                     '6': '3',
                     '7': '2',
                     '8': '1'}
        try:
            return transform[str(y)]
        except KeyError:
            pass

    def convert_point_to_square(self, x, y):
        x = int(x)
        y = int(y)
        x_red = int((x - 50) / 74) + 1
        y_red = self.y_transform(int((y + 50) / 74))
        logging.debug("Converting point to square {} {}".format(x_red, y_red))
        return self.text_to_square("%s%s" % (self.convert_x_axis_to_letter(x_red), y_red))

    def convert_square_to_point(self, square):
        coords = square.coords
        x = ((int(self.convert_x_axis_to_numeral(coords[0])) - 1)* 74) + 50
        y = ((int(self.y_transform(coords[1])) - 1) * 74) + 50
        return (x, y)

    def get_coords(self, s, d):
        s_x = int(self.convert_x_axis_to_numeral(s[0]))
        s_y = int(s[1])
        d_x = int(self.convert_x_axis_to_numeral(d[0]))
        d_y = int(d[1])
        return (s_x, s_y, d_x, d_y)

    def path(self, source_square, destination_square):
        if self.text_to_square(source_square).piece.name() == 'knight':
            return []

        path = [self.text_to_square(source_square)]
        s_x, s_y, d_x, d_y = self.get_coords(source_square, destination_square)
        x_interval = self.get_interval(s_x, d_x)
        y_interval = self.get_interval(s_y, d_y)
        x_pos = s_x
        y_pos = s_y
        logging.debug("Converting x axis from position to letter {} {} ".format(x_pos, y_pos))
        if x_interval == 0:
            while y_pos != d_y:
                x_pos = x_pos + x_interval
                y_pos = y_pos + y_interval
                path.append(self.text_to_square("%s%s" % (self.convert_x_axis_to_letter(x_pos), y_pos)))
        elif y_interval == 0:
            while x_pos != d_x:
                x_pos = x_pos + x_interval
                y_pos = y_pos + y_interval
                path.append(self.text_to_square("%s%s" % (self.convert_x_axis_to_letter(x_pos), y_pos)))
        else:
            while x_pos != d_x:
                while y_pos != d_y:
                    x_pos = x_pos + x_interval
                    y_pos = y_pos + y_interval
                    path.append(self.text_to_square("%s%s" % (self.convert_x_axis_to_letter(x_pos), y_pos)))
        
        return path

    def get_interval(self, s, d):
        difference = d - s
        if difference > 0:
            return 1
        elif difference < 0:
            return -1
        else:
            return 0

    def convert_x_axis_to_numeral(self, x):
        x = x.replace("a","1")
        x = x.replace("b","2")
        x = x.replace("c","3")
        x = x.replace("d","4")
        x = x.replace("e","5")
        x = x.replace("f","6")
        x = x.replace("g","7")
        x = x.replace("h","8")
        return x

    def convert_x_axis_to_letter(self, x):
        if x <1 or x > 8:
            raise InvalidMoveException("Invalid X axis")

        if x == 1:
            return "a"
        elif x == 2:
            return "b"
        elif x == 3:
            return "c"
        elif x == 4:
            return "d"
        elif x == 5:
            return "e"
        elif x == 6:
            return "f"
        elif x == 7:
            return "g"
        elif x == 8:
            return "h"

    def __str__(self):
        state = "\n8[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a8, self.b8, self.c8, self.d8, self.e8, self.f8, self.g8, self.h8)
        state = state + "7[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a7, self.b7, self.c7, self.d7, self.e7, self.f7, self.g7, self.h7)
        state = state + "6[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a6, self.b6, self.c6, self.d6, self.e6, self.f6, self.g6, self.h6)
        state = state + "5[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a5, self.b5, self.c5, self.d5, self.e5, self.f5, self.g5, self.h5)
        state = state + "4[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a4, self.b4, self.c4, self.d4, self.e4, self.f4, self.g4, self.h4)
        state = state + "3[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a3, self.b3, self.c3, self.d3, self.e3, self.f3, self.g3, self.h3)
        state = state + "2[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a2, self.b2, self.c2, self.d2, self.e2, self.f2, self.g2, self.h2)
        state = state + "1[%s][%s][%s][%s][%s][%s][%s][%s]\n" % (self.a1, self.b1, self.c1, self.d1, self.e1, self.f1, self.g1, self.h1)
        state = state + "  a   b   c   d   e   f   g   h  \n"
        return state
