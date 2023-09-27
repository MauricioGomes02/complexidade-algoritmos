## Justificativa

A implementação do tipo abstrato de dado Set deve atententar-se na utilização de uma estrutura que garante uma pesquisa eficiente, devido o fato de que todas as operações dependerem da realização de pesquisas para validar a presença de um elemento no conjunto.

As estruturas AVL Tree e Hash Table são bons exemplos de estruturas com performance de consulta, inserção e remoção. Entretanto, devido a melhor complexidade em caso médio, a estrutura escolhida para a implementação é a Hash Table.

Mesmo com possibilidade de colisão e uso de uma quantidade maior de memória, como a Hash Table trata-se de uma implementação apenas para estudo, a quantidade de itens não será significamente grande para atingir o pior caso.

## Operações

### Contain

Responsável por informar se um determinado elemento está presente no conjunto

|                                      | HashTable | AVLTree      |
|--------------------------------------|-----------|--------------|
| **Complexidade De Tempo Caso Médio** | $O(1)$    | $O(\log{n})$ |
| **Complexidade De Espaço**           | $O(n)$    | $O(n)$       |

### Insert

Responsável por inserir um elemento no conjunto

|                                      | HashTable | AVLTree      |
|--------------------------------------|-----------|--------------|
| **Complexidade De Tempo Caso Médio** | $O(1)$    | $O(\log{n})$ |
| **Complexidade De Espaço**           | $O(n)$    | $O(n)$       |


### Remove

Responsável por inserir um elemento no conjunto

|                                      | HashTable | AVLTree      |
|--------------------------------------|-----------|--------------|
| **Complexidade De Tempo Caso Médio** | $O(1)$    | $O(\log{n})$ |
| **Complexidade De Espaço**           | $O(n)$    | $O(n)$       |


### Intersect

Responsável por realizar a operação de intersecção de conjuntos. A complexidade de tempo e espaço é de $O(n)$ devido o fato de ser necessário executar uma varredura completa em um conjunto para verificar se cada elemento possui um correspondente no outro conjunto informado. Em caso positivo, é necessário inserir esse novo elemento no conjunto resultante

- Operação de inserção e pesquisa possuem complexidade $O(1)$. Portanto, não impactam na complexidade de limite superior

### Union

Responsável por realizar a operação de união de conjuntos. A complexidade de tempo e espaço é de $O(n)$ + $O(m)$ devido o fato de ser necessário executar uma varredura completa em ambos os conjuntos que podem possuir tamanhos distintos, adicionando seus elementos, evitando a duplicação de dados

- Operação de inserção e pesquisa possuem complexidade $O(1)$. Portanto, não impactam na complexidade de limite superior

### Difference

Responsável por realizar a operação de diferença de conjuntos. A complexidade de tempo e espaço é de $O(n)$ devido o fato de ser necessário executar uma varredura completa em um conjunto para verificar se cada elemento não existe no outro conjunto informado. Em caso positivo, é necessário inserir esse novo elemento no conjunto resultante

- Operação de inserção e pesquisa possuem complexidade $O(1)$. Portanto, não impactam na complexidade de limite superior