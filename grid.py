import pygame

import numpy as np
from wall import Wall
from exit import Exit, Start
from player import Player
from gate import Gate, gates_ids_inv
from floor import Floor

def generate_grid(NUMBER_OF_TILES_X, NUMBER_OF_TILES_Y):
    grid = np.zeros((NUMBER_OF_TILES_Y,NUMBER_OF_TILES_X), dtype=int)
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if((i==0 or j==0) or (i==(grid.shape[0]-1) or j==(grid.shape[1]-1))):
                grid[i,j] = 1
    return(grid)

def generate_lvl1():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1, -1,  0,  0,  0,  0,  0,  2,  1,  1,  1,  1],
       [ 1,  1,  1,  0,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  0,  6,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvl2():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  0,  0,  0,  5,  5,  0,  0,  1,  1,  1],
       [ 1, -1,  0,  0,  1,  1,  1,  1,  0,  0,  0,  2],
       [ 1,  1,  0,  0,  0,  6,  0,  0,  0,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvl3():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1],
       [ 1, -1,  0,  0,  1,  1,  1,  1,  0,  0,  6,  2],
       [ 1,  1,  0,  0,  6,  0,  0,  6,  0,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvl4():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1, -1,  0,  0,  6,  0,  0,  6,  4,  0,  6,  2],
       [ 1,  1,  1,  1,  1,  1,  1,  0,  0,  0,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvl5():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  -1,  0,  5,  9,  9,  5,  0,  2,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvlX():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,   1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,   0,  6,  0,  1,  0,  0,  0,  1,  1,  1,  1],
       [ 1,  -1,  1,  0,  1,  0,  1,  0,  1,  0,  2,  1],
       [ 1,   1,  1,  0,  1,  0,  1,  0,  1,  0,  1,  1],
       [ 1,   1,  1,  0,  0,  0,  1,  0,  0,  0,  1,  1],
       [ 1,   1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvl6():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1, -1,  0,  0,  0,  0,  0,  2,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def generate_lvlXX():
    grid = np.array([[ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  0,  0,  6,  9,  5,  0,  0,  1,  1,  1],
       [ 1,  -1,  0,  0,  5,  9,  5,  0,  0,  2,  1,  1],
       [ 1,  1,  0,  0,  8,  6,  9,  0,  0,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1],
       [ 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1]])
    return(grid.T)

def grid_2_walls(grid, TILE_SIZE):
    walls = pygame.sprite.Group()
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==1):
                walls.add(Wall((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE))
    return(walls)

def grid_2_floors(grid, TILE_SIZE):
    walls = pygame.sprite.Group()
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==0):
                walls.add(Floor((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE))
            if(grid[i,j]>=4):
                walls.add(Floor((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE))
            if(grid[i,j]==2):
                walls.add(Floor((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE))
    return(walls)

def grid_2_exit(grid, TILE_SIZE):
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==2):
                exit = Exit((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE=TILE_SIZE)
    return(exit)

def grid_2_start(grid, TILE_SIZE):
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==-1):
                player = Start((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE=TILE_SIZE)
    return(player)

def grid_2_player(grid, TILE_SIZE):
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==-1):
                player = Player((i*TILE_SIZE, j*TILE_SIZE))
    return(player)

def grid_2_gates(grid, TILE_SIZE):
    gates = pygame.sprite.Group()
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if grid[i,j] in gates_ids_inv.keys():
                gates.add(Gate(gates_ids_inv[grid[i,j]], (i*TILE_SIZE, j*TILE_SIZE)))
    return(gates)