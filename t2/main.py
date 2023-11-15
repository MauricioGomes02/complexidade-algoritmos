import os
from queue import PriorityQueue

def get_current_directory():
    return os.getcwd()

def get_file_lines(file_path):
    with open(file_path, 'r') as file:
        lines = file.readlines()
        return lines
    
def generate_map(file_path):
    file_lines = get_file_lines(file_path)
    if len(file_lines) < 3:
        raise Exception('A definição do mapa deve possuir pelo menos 3 linhas!')
    
    first_line = file_lines[0]
    (width, height) = get_size(first_line)

    second_line = file_lines[1]
    (initial_position_x, initial_position_y) = get_initial_position(second_line, width, height)
    
    map_lines = get_map_lines(file_lines[2:], width, height, initial_position_x, initial_position_y)
    return (width, height, initial_position_x, initial_position_y, map_lines)
    
def get_size(first_line):
    if len(first_line) <= 2:
        raise Exception('A primeira linha deve possuir pelo menos 3 caracteres')
    
    elements = first_line.split()
    if len(elements) != 2:
        raise Exception('A primeira linha deve possuir pelo menos 2 elementos (largura, altura)')
    
    width = int(elements[0])
    height = int(elements[1])
    return (width, height)

def get_initial_position(second_line, width, height):
    if len(second_line) <= 2:
        raise Exception('A segunda linha deve possuir pelo menos 3 caracteres')
    
    elements = second_line.strip().split()
    if len(elements) != 2:
        raise Exception('A segunda linha deve possuir pelo menos 2 elementos (x, y)')
    
    x = int(elements[0])
    y = int(elements[1])

    if x < 0 and x > width - 1:
        raise Exception('O eixo x da posição inicial não pode ser maior que a quantidade de colunas')
    
    if y < 0 and y > height - 1:
        raise Exception('O eixo y da posição inicial não pode ser maior que a quantidade de linhas')

    return (x, y)

def get_map_lines(lines, width, height, x, y):
    if len(lines) != height:
        raise Exception('A quantidade de linhas do mapa não pode ser diferente da sua altura')
    
    x_count = 0

    map_lines = []
    
    for line in lines:
        elements_line = line.strip().split()

        if len(elements_line) != width:
            raise Exception('A quantidade de colunas do mapa não pode ser diferente da sua largura')
        
        map_line = []
        y_count = 0
        for element in elements_line:
            element = int(element)
            if element < -1:
                raise Exception('Os valores não podem ser menores que -1')
            
            if element == -1 and (x == x_count and y == y_count):
                raise Exception('A posição inicial não pode ser em um lugar inacessível')
            
            map_line.append(element)

            y_count += 1
        
        x_count += 1
        map_lines.append(map_line)

    return map_lines

def memoization(subproblem):
    def get_subproblem_solution(*args):
        arguments_key = tuple(args)
        if arguments_key not in memo:
            memo[arguments_key] = subproblem(*args)

        return memo[arguments_key]

    memo = {}
    return get_subproblem_solution

def get_neighbors(current, matrix):
    (column, line) = current
    neighbors = []

    # top
    if line - 1 >= 0 and matrix[line - 1][column] >= 0:
        top_neighbor = (column, line - 1)
        neighbors.append(top_neighbor)

    # left
    if column - 1 >= 0 and matrix[line][column - 1] >= 0:
        left_neighbor = (column - 1, line)
        neighbors.append(left_neighbor)

    # bottom
    if line + 1 < len(matrix) and matrix[line + 1][column] >= 0:
        bottom_neighbor = (column, line + 1)
        neighbors.append(bottom_neighbor)

    # right
    if column + 1 < len(matrix[0]) and matrix[line][column + 1] >= 0:
        right_neighbor = (column + 1, line)
        neighbors.append(right_neighbor)

    return neighbors

def get_cost(vertice, matrix):
    (column, line) = vertice
    return matrix[line][column]

def heuristic(_from, to):
    (from_x, from_y) = _from
    (to_x, to_y) = to
    return abs(from_x - to_x) + abs(from_y - to_y)

def a_star(matrix, start, end):
    fronteir = PriorityQueue()
    (start_x, start_y) = start
    start_cost = matrix[start_y][start_x]
    fronteir.put(start, start_cost)

    came_from = dict()
    cost_so_far = dict()

    came_from[start] = None
    cost_so_far[start] = start_cost

    while not fronteir.empty():
        current = fronteir.get()

        for next in get_neighbors(current, matrix):
            new_cost = cost_so_far[current] + get_cost(next, matrix) + 1
            if next not in cost_so_far or new_cost < cost_so_far[next]:
                cost_so_far[next] = new_cost
                priority = new_cost + heuristic(end, next)
                fronteir.put(next, priority)
                came_from[next] = current

    path = [end]
    current_path = came_from[end]
    path.append(current_path)
    while current_path != start:
        current_path = came_from[current_path]
        path.append(current_path)

    print_path(path, matrix, start, end)

    cost = cost_so_far[end]
    print_exit(cost, path)

def print_path(path, matrix, start, end):
    vertices_path = dict()

    v_count = 1
    while v_count < len(path):
        vertice = path[v_count]
        (v_x, v_y) = vertice
        previous_vertice = path[v_count - 1]
        (p_x, p_y) = previous_vertice
        value = None
        if v_x - p_x == 1:
            value = ' < '
        elif v_x - p_x == -1:
            value = ' > '
        elif v_y - p_y == 1:
            value = ' ^ '
        else:
            value = ' v '

        vertices_path[vertice] = value
        v_count += 1

    count_line = 0
    while count_line < len(matrix):
        table_line = ''
        count_column = 0
        while count_column < len(matrix[0]):
            cost = matrix[count_line][count_column]

            value = None

            if (count_column, count_line) == start:
                value = ' S '
            elif (count_column, count_line) == end:
                value = ' E '
            elif (count_column, count_line) in vertices_path:
                value = vertices_path[(count_column, count_line)]
            elif cost < 0:
                value = ' ■ '
            else:
                value = ' - '

            table_line += value
            count_column += 1
        print(f'{table_line}\n')
        count_line += 1

def print_exit(cost, coordinates):
    exit = f'{cost}'
    count = len(coordinates) - 1
    while count > 0:
        coordinate = coordinates[count]
        (x, y) = coordinate
        exit += f'  {x},{y}'
        count -= 1
    print(exit)
        
if __name__ == '__main__':
    current_directory = get_current_directory()
    file_path = f'{current_directory}/map.txt'
    (width, height, x, y, lines) = generate_map(file_path)
    start = (x, y)
    
    end = None
    while True:
        _input = input('Informe a coordenada de destino: (formato: \'x y\'): ')
        elements = _input.strip().split()
        if len(elements) != 2:
            continue

        try:
            end_x = int(elements[0])
            end_y = int(elements[1])

            if lines[end_y][end_x] < 0:
                print('O seu destino não pode ser em um local inacessível')
                continue

            if end_x < 0 or end_x >= width or end_y < 0 or end_y >= height:
                print('Coordenada inexistente!')
                continue

            end = (end_x, end_y)

            a_star(lines, start, end)
            break
        except:
            print('Informe valores válidos para as coordenadas')
            continue