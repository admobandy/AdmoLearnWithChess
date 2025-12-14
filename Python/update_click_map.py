import json

square_size = 74
board_offset_x = 45
board_offset_y = 45

click_map = {}

for row in range(8):
    for col in range(8):
        col_letter = chr(ord('a') + col)  # 'a'..'h'
        row_number = 8 - row  # top row = 8
        square_name = f"{col_letter}{row_number}"  # string key

        # Compute center of the square
        center_x = board_offset_x + col * square_size + square_size // 2
        center_y = board_offset_y + row * square_size + square_size // 2

        # Save as strings
        click_map[str(square_name)] = {"x": str(center_x), "y": str(center_y)}

with open("click_map.json", "w") as f:
    json.dump(click_map, f, indent=4)
