import pygame

class Button():
    def __init__(self, screen, x, y, image, scale):
        self.screen = screen
        width = image.get_width()
        height = image.get_height()
        self.image = pygame.transform.scale_by(image, scale)
        self.rect = self.image.get_rect()
        self.rect.center = (x,y)
        self.clicked = False
        self.clicked_timer = 0

    def timer(self):
        self.clicked_timer += 1
        if self.clicked_timer == 10:
            return True

    def draw(self):
        action = False
        # get mouse position
        pos = pygame.mouse.get_pos()

        # check mouseover and clicked positions
        image = self.image
        if self.rect.collidepoint(pos):
            image = pygame.transform.scale_by(self.image, 1.05)
            if pygame.mouse.get_pressed()[0] and self.clicked == False:
                action = True
                image = self.image
                self.clicked = True
            if pygame.mouse.get_pressed()[0] == 0:
                self.clicked = False
        
        # draw button on screen
        self.screen.blit(image, self.rect)

        return action
    

class HealthBar():
    def __init__(self,x,y,w,h,max_hp):
        self.x = x
        self.y = y
        self.w = w
        self.h = h
        self.hp = max_hp
        self.max_hp = max_hp

        self.image = pygame.image.load('Assets/Miscelanous/health_bar.png').convert_alpha()
        self.rect = self.image.get_rect(bottomright = (x,y))

    def draw(self, surface):
        ratio = self.hp / self.max_hp
        pygame.draw.rect(surface, (243,97,255), (self.x-self.w-10, self.y-self.h-12, self.w * ratio, int(self.h/2))) # lighter pink
        pygame.draw.rect(surface, (219,65,195), (self.x-self.w-10, self.y+int(self.h/2)-self.h-12, self.w * ratio, int(self.h/2))) # darker pink
        surface.blit(self.image, self.rect)


class Message():
    def __init__(self, screen, font, text, x, y):
        self.message = font.render(text,False,(255,255,255))
        self.rect = self.message.get_rect(midleft = (x,y))
        screen.blit(self.message, self.rect)


class DrawImage():
    def __init__(self, screen, image, scale, x, y):
        image = pygame.image.load(image).convert_alpha()
        image = pygame.transform.scale_by(image,scale)
        rect = image.get_rect(center = (x, y))
        screen.blit(image, rect)
