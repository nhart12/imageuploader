package api

import (
	"github.com/google/uuid"
	"github.com/nhart12/imageuploader/pkg/core"
)

type ImageService struct {
	repository core.Repository
}

func (s ImageService) SearchImagesByTitle(title string) ([]*core.Image, error) {
	return s.repository.SearchByTitle(title)
}

func (s ImageService) SearchImagesByTags(tags []string) ([]*core.Image, error) {
	return s.repository.SearchByTags(tags)
}

func (s ImageService) DeleteImage(id uuid.UUID) {
	s.repository.Delete(id)
}

func (s ImageService) CreateImage(fileName string, imgData []byte) uuid.UUID {
	img := core.Image{
		Data:     imgData,
		FileName: fileName,
		Id:       uuid.New(),
	}
	s.repository.Create(&img)
	return img.Id
}

func (s ImageService) SetImageMetadata(id uuid.UUID, dto core.ImageDto) {
	img, _ := s.Get(id)
	img.Description = dto.Description
	img.Tags = dto.Tags
	img.Title = dto.Title
}

func (s ImageService) Get(id uuid.UUID) (*core.ImageDto, error) {
	img, err := s.repository.Get(id)
	if err != nil {
		return nil, err
	}
	imgDto := core.ImageDto{
		Description: img.Description,
		FileName:    img.FileName,
		Tags:        img.Tags,
		Title:       img.Title,
	}
	return &imgDto, nil
}

func (s ImageService) GetAll() ([]*core.Image, error) {
	return s.repository.GetAll()
}

func mapDto(img *core.Image) *core.ImageDto {
	return &core.ImageDto{
		Description: img.Description,
		FileName:    img.FileName,
		Tags:        img.Tags,
		Title:       img.Title,
	}
}
func NewImageService(repo core.Repository) ImageService {
	return ImageService{repository: repo}
}
