package core

import (
	"github.com/google/uuid"
)

type Image struct {
	Id          uuid.UUID
	Tags        []string
	Title       string
	Description string
	Data        []byte
	FileName    string
	ContentType string
}

type ImageDto struct {
	Id          uuid.UUID `json:"id"`
	Tags        []string  `json:"tags"`
	Title       string    `json:"title"`
	Description string    `json:"description"`
	FileName    string    `json:"fileName"`
	Path        string    `json:"path"`
}
