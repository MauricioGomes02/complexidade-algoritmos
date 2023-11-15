import os
from queue import PriorityQueue

def get_current_directory():
    return os.getcwd()

def get_map_file_path(current_directory):
    return f'{current_directory}/map.txt'       

def get_file_lines(file_path):
    with open(file_path, 'r') as file:
        lines = file.readlines()
        return lines
    
def get_first_line(line):
    if len(line) <= 2:
        raise Exception('A primeira linha deve possuir pelo menos 3 caracteres')
    
    elements = line.split()
    if len(elements) != 2:
        raise Exception('A primeira linha deve possuir pelo menos 2 elementos (largura, altura)')
    
    width = int(elements[0])
    height = int(elements[1])

    size = dict()
    size['width'] = width
    size['height'] = height
    return size

def get_second_line(line, size, matrix):
    width = size['width']
    height = size['height']

    if len(line) <= 2:
        raise Exception('A segunda linha deve possuir pelo menos 3 caracteres')
    
    elements = line.strip().split()
    if len(elements) != 2:
        raise Exception('A segunda linha deve possuir pelo menos 2 elementos (x, y)')
    
    x = int(elements[0])
    y = int(elements[1])

    if x < 0 or y < 0:
        raise Exception('Os valores da coordenada não podem ser menores que zero')

    if x > width - 1:
        raise Exception('O eixo x da posição inicial não pode ser maior que a quantidade de colunas')
    
    if y > height - 1:
        raise Exception('O eixo y da posição inicial não pode ser maior que a quantidade de linhas')
    
    if matrix[y][x] < 0:
        raise Exception('A posição inicial não pode estar em um local inacessível')

    return (x, y)

def get_end(size, matrix):
    while True:
        _input = input('Informe a coordenada de destino: (formato: \'x y\'): ')
        elements = _input.strip().split()
        if len(elements) != 2:
            print('A entrada deve possuir apenas 2 elementos')
            continue

        try:
            vertice = (int(elements[0]), int(elements[1]))

            (x, y) = vertice

            width = size['width']
            height = size['height']

            if x < 0 or y < 0:
                raise Exception('Os valores da coordenada não podem ser menores que zero')

            if x > width - 1:
                raise Exception('O eixo x da posição final não pode ser maior que a quantidade de colunas')
            
            if y > height - 1:
                raise Exception('O eixo y da posição final não pode ser maior que a quantidade de linhas')
            
            if matrix[y][x] < 0:
                raise Exception('A posição final não pode estar em um local inacessível')

            if x < 0 or y < 0:
                raise Exception('Os valores da coordenada não podem ser menores que zero')

            return vertice
            
        except Exception as exception:
            print(exception)

def get_map_matrix(lines, size):
    width = size['width']
    height = size['height']

    if len(lines) != height:
        raise Exception('A quantidade de linhas do mapa não pode ser diferente da sua altura')
    
    matrix = []
    line_count = 0
    while line_count < height:
        matrix.append([])
        line = lines[line_count]
        elements = line.strip().split()
        if len(elements) != width:
            raise Exception('A quantidade de colunas do mapa não pode ser diferente da sua largura')
        
        column_count = 0
        while column_count < width:
            cost = int(elements[column_count])
            if cost < -1:
                raise Exception('Os valores não podem ser menores que -1')
            
            matrix[line_count].append(cost)
            column_count += 1
        line_count += 1

    return matrix

def get_map(file_lines):
    file_lines = get_file_lines(file_path)
    if len(file_lines) < 3:
        raise Exception('A definição do mapa deve possuir pelo menos 3 linhas!')
    
    size = get_first_line(file_lines[0])
    map_matrix = get_map_matrix(file_lines[2:], size)
    start = get_second_line(file_lines[1], size, map_matrix)
    end = get_end(size, map_matrix)
    
    map = dict()
    map['size'] = size
    map['start'] = start
    map['end'] = end
    map['matrix'] = map_matrix

    return map

def memoization(subproblem):
    def get_subproblem_solution(*args):
        arguments_key = tuple(args)
        if arguments_key not in memo:
            memo[arguments_key] = subproblem(*args)

        return memo[arguments_key]

    memo = {}
    return get_subproblem_solution

def a_star(map):
    start = map['start']
    end = map['end']
    matrix = map['matrix']
    frontier = PriorityQueue()
    start_cost = get_cost(matrix, start)
    frontier.put(start, start_cost)
    came_from = dict()
    cost_so_far = dict()
    came_from[start] = None
    cost_so_far[start] = start_cost

    while not frontier.empty():
        current_vertice = frontier.get()

        for next_vertice in get_neighbors(matrix, current_vertice):
            new_cost = cost_so_far[current_vertice] + get_cost(matrix, next_vertice)
            if next_vertice not in cost_so_far or new_cost < cost_so_far[next_vertice]:
                cost_so_far[next_vertice] = new_cost
                priority = new_cost + heuristic(end, next_vertice)
                frontier.put(next_vertice, priority)
                came_from[next_vertice] = current_vertice

    result = dict()
    result['cost'] = cost_so_far[end]
    result['path'] = get_path(came_from, end, start)

    return result

def get_path(came_from, end, start):
    path = [end]
    current_path = came_from[end]
    path.append(current_path)
    while current_path != start:
        current_path = came_from[current_path]
        path.append(current_path)

    path.reverse()
    return path

def get_cost(matrix, vertice):
    (x, y) = vertice
    return matrix[y][x]

def get_neighbors(matrix, vertice):
    (x, y) = vertice

    neighbors = []

    # top
    if y - 1 >= 0 and matrix[y - 1][x] >= 0:
        top_neighbor = (x, y - 1)
        neighbors.append(top_neighbor)

    # left
    if x - 1 >= 0 and matrix[y][x - 1] >= 0:
        left_neighbor = (x - 1, y)
        neighbors.append(left_neighbor)

    # bottom
    if y + 1 < len(matrix) and matrix[y + 1][x] >= 0:
        bottom_neighbor = (x, y + 1)
        neighbors.append(bottom_neighbor)

    # right
    if x + 1 < len(matrix[0]) and matrix[y][x + 1] >= 0:
        right_neighbor = (x + 1, y)
        neighbors.append(right_neighbor)

    return neighbors

def heuristic(_from, to):
    (from_x, from_y) = _from
    (to_x, to_y) = to

    return abs(from_x - to_x) + abs(from_y - to_y)

def print_path(path, map):
    best_path = get_best_path(path)
    line_count = 0
    size = map['size']
    width = size['width']
    height = size['height']
    matrix = map['matrix']
    start = map['start']
    end = map['end']
    print()
    while line_count < height:
        column_count = 0
        line = ''
        while column_count < width:
            coordinate = (column_count, line_count)
            cost = matrix[line_count][column_count]
            if coordinate == start:
                line += ' S '
            elif coordinate == end:
                line += ' E '
            elif coordinate in best_path:
                line += best_path[coordinate]
            elif cost < 0:
                line += ' ■ '
            else:
                line += ' - '

            column_count += 1

        print(line + '\n')
        line_count += 1

def print_exit(cost, coordinates):
    exit = f'{cost}'
    count = len(coordinates) - 1
    while count > 0:
        coordinate = coordinates[count]
        (x, y) = coordinate
        exit += f'  {x},{y}'
        count -= 1
    print(exit)

def get_best_path(path):
    best_path = dict()

    vertice_count = len(path) - 1

    while vertice_count > 0:
        vertice = path[vertice_count]
        (vertice_x, vertice_y) = vertice
        previous_vertice = path[vertice_count - 1]
        (previous_vertice_x, previous_vertice_y) = previous_vertice
        difference = (vertice_x - previous_vertice_x, vertice_y - previous_vertice_y)
        symbols = {
            (-1, 0): ' < ',
            (1, 0): ' > ',
            (0, 1): ' v ',
            (0, -1): ' ^ '
        }
        value = symbols[difference]
        best_path[previous_vertice] = value
        vertice_count -= 1
    
    return best_path
        
if __name__ == '__main__':
    current_directory = get_current_directory()
    file_path = get_map_file_path(current_directory)
    file_lines = get_file_lines(file_path)
    map = get_map(file_lines)
    result = a_star(map)

    path = result['path']
    cost = result['cost']
    print_path(path, map)
    print_exit(cost, path)