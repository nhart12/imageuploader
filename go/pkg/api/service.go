package api

import (
	"github.com/google/uuid"
	"github.com/nhart12/imageuploader/go/pkg/core"
)

type ImageService struct {
	repository core.Repository
}

func (s ImageService) SearchImagesByTitle(title string) ([]*core.ImageDto, error) {
	imgs, _ := s.repository.SearchByTitle(title)
	return mapDtos(imgs), nil
}

func (s ImageService) SearchImagesByTags(tags []string) ([]*core.ImageDto, error) {
	imgs, _ := s.repository.SearchByTags(tags)
	return mapDtos(imgs), nil
}

func (s ImageService) DeleteImage(id uuid.UUID) {
	s.repository.Delete(id)
}

func (s ImageService) CreateImage(fileName string, imgData []byte, contentType string) uuid.UUID {
	img := core.Image{
		Data:        imgData,
		FileName:    fileName,
		Id:          uuid.New(),
		ContentType: contentType,
	}
	s.repository.Create(&img)
	return img.Id
}

func (s ImageService) SetImageMetadata(id uuid.UUID, dto core.ImageDto) *core.ImageDto {
	img, _ := s.repository.Get(id)
	if img == nil {
		return nil
	}
	img.Description = dto.Description
	img.Tags = dto.Tags
	img.Title = dto.Title
	return mapDto(img)
}

func (s ImageService) Get(id uuid.UUID) (*core.ImageDto, error) {
	img, err := s.repository.Get(id)
	if err != nil {
		return nil, err
	}
	if img == nil {
		return nil, nil
	}
	return mapDto(img), nil
}

func (s ImageService) GetAll() ([]*core.ImageDto, error) {
	imgs, err := s.repository.GetAll()
	if err != nil {
		return nil, err
	}

	return mapDtos(imgs), nil
}

func (s ImageService) GetAllTags() ([]string, error) {
	imgs, err := s.repository.GetAll()
	if err != nil {
		return nil, err
	}
	if len(imgs) == 0 {
		return []string{}, nil
	}
	tags := make(map[string]bool)
	for _, img := range imgs {
		for _, tag := range img.Tags {
			tags[tag] = true
		}
	}
	tagSlice := make([]string, 0, len(tags))
	for tag := range tags {
		tagSlice = append(tagSlice, tag)
	}
	return tagSlice, nil
}

func (s ImageService) GetFileData(id uuid.UUID) ([]byte, string) {
	img, _ := s.repository.Get(id)
	return img.Data, img.ContentType
}

func mapDtos(imgs []*core.Image) []*core.ImageDto {
	response := []*core.ImageDto{}
	if len(imgs) == 0 {
		return response
	}
	for _, img := range imgs {
		response = append(response, mapDto(img))
	}
	return response
}

func mapDto(img *core.Image) *core.ImageDto {
	return &core.ImageDto{
		Id:          img.Id,
		Description: img.Description,
		FileName:    img.FileName,
		Tags:        img.Tags,
		Title:       img.Title,
		Path:        img.Id.String() + "/" + img.FileName,
	}
}
func NewImageService(repo core.Repository) ImageService {
	return ImageService{repository: repo}
}
