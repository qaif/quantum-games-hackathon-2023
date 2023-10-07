import pygame

import numpy as np
from wall import Wall
from exit import Exit
from player import Player
from gate import Gate

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
       [ 1,  1,  0,  0,  0,  5,  0,  0,  0,  1,  1,  1],
       [ 1, -1,  0,  0,  1,  1,  1,  0,  0,  0,  2,  1],
       [ 1,  1,  0,  0,  0,  6,  0,  0,  0,  1,  1,  1],
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

def grid_2_exit(grid, TILE_SIZE):
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==2):
                exit = Exit((i*TILE_SIZE, j*TILE_SIZE), TILE_SIZE=TILE_SIZE)
    return(exit)

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
            if(grid[i,j]==5):
                gates.add(Gate('H', (i*TILE_SIZE, j*TILE_SIZE)))
            if(grid[i,j]==6):
                gates.add(Gate('X', (i*TILE_SIZE, j*TILE_SIZE)))
    return(gates)