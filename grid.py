import pygame

import numpy as np
from wall import Wall

def generate_grid(NUMBER_OF_TILES_X, NUMBER_OF_TILES_Y):
    grid = np.zeros((NUMBER_OF_TILES_Y,NUMBER_OF_TILES_X), dtype=int)
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if((i==0 or j==0) or (i==(grid.shape[0]-1) or j==(grid.shape[1]-1))):
                grid[i,j] = 1
    return(grid)

def grid_2_walls(grid, TILE_SIZE):
    walls = pygame.sprite.Group()
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if(grid[i,j]==1):
                walls.add(Wall((i*TILE_SIZE, j*TILE_SIZE)))
    return(walls)