import pygame
import io
import matplotlib.pyplot as plt

# Initialize Pygame
pygame.init()

# Set up display
screen_width, screen_height = 800, 600
screen = pygame.display.set_mode((screen_width, screen_height))
pygame.display.set_caption("Matplotlib Image in Pygame")

# Generate a Matplotlib figure
fig, ax = plt.subplots()
ax.plot([0, 1, 2, 3, 4], [0, 1, 4, 9, 16])

# Save the Matplotlib figure to a BytesIO object
buffer = io.BytesIO()
plt.savefig(buffer, format="png")
buffer.seek(0)

# Load the image as a Pygame surface
image_surface = pygame.image.load(buffer)

# Main game loop
running = True
while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

    # Clear the screen
    screen.fill((0, 0, 0))

    # Render the image surface on the screen
    screen.blit(image_surface, (100, 100))

    # Update the display
    pygame.display.flip()

# Quit Pygame
pygame.quit()