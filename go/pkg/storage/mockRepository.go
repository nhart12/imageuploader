package storage

import (
	"github.com/google/uuid"
	"github.com/nhart12/imageuploader/go/pkg/core"
)

var images = make(map[uuid.UUID]*core.Image)

type MockRepository struct {
}

func (r MockRepository) SearchByTitle(title string) ([]*core.Image, error) {
	matches := []*core.Image{}
	for _, img := range images {
		if img.Title == title {
			matches = append(matches, img)
		}
	}
	return matches, nil
}

func (r MockRepository) SearchByTags(tags []string) ([]*core.Image, error) {
	matches := []*core.Image{}
	for _, img := range images {
		matched := false
		for _, tag := range img.Tags {
			for _, searchTag := range tags {
				if tag == searchTag {
					matched = true
					matches = append(matches, img)
					break
				}
			}
			if matched {
				break
			}
		}
	}
	return matches, nil
}

func (r MockRepository) Delete(id uuid.UUID) {
	delete(images, id)
}

func (r MockRepository) Create(img *core.Image) {
	images[img.Id] = img
}

func (r MockRepository) Get(id uuid.UUID) (*core.Image, error) {

	return images[id], nil
}
func (r MockRepository) GetAll() ([]*core.Image, error) {
	imgSlice := make([]*core.Image, 0, len(images))
	for _, img := range images {
		imgSlice = append(imgSlice, img)
	}
	return imgSlice, nil
}

func NewMockRepository() core.Repository {
	return MockRepository{}
}
