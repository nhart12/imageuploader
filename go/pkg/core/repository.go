package core

import (
	"github.com/google/uuid"
)

type Repository interface {
	SearchByTitle(title string) ([]*Image, error)
	SearchByTags(tags []string) ([]*Image, error)
	Delete(id uuid.UUID)
	Create(img *Image)
	Get(id uuid.UUID) (*Image, error)
	GetAll() ([]*Image, error)
}
