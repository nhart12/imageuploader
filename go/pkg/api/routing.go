package api

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"

	"github.com/google/uuid"
	"github.com/gorilla/mux"
	"github.com/nhart12/imageuploader/go/pkg/core"
)

type ApiHandler struct {
	Router  http.Handler
	service ImageService
}

func NewRouter(repo core.Repository) ApiHandler {
	apiHandler := ApiHandler{service: NewImageService(repo)}
	httpRouter := mux.NewRouter().StrictSlash(true)
	httpRouter.HandleFunc("/api/images/search", apiHandler.searchImages).Methods("GET")
	httpRouter.HandleFunc("/api/images/{id}/metadata", apiHandler.createImageMetadata).Methods("POST")
	httpRouter.HandleFunc("/api/images/{id}", apiHandler.getImageMetadata).Methods("GET")
	httpRouter.HandleFunc("/api/images/{id}", apiHandler.deleteImage).Methods("DELETE")
	httpRouter.HandleFunc("/api/images/{id}/{fileName}", apiHandler.downloadImage).Methods("GET")
	httpRouter.HandleFunc("/api/images", apiHandler.uploadImage).Methods("POST")
	httpRouter.HandleFunc("/api/images", apiHandler.getAllImages).Methods("GET")
	httpRouter.HandleFunc("/api/tags", apiHandler.getAllTags).Methods("GET")
	apiHandler.Router = httpRouter
	return apiHandler
}

func (h ApiHandler) searchImages(w http.ResponseWriter, r *http.Request) {
	tags := r.URL.Query()["tags"]
	imgs, _ := h.service.SearchImagesByTags(tags)
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(imgs)
}
func (h ApiHandler) getAllImages(w http.ResponseWriter, r *http.Request) {
	imgs, _ := h.service.GetAll()
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(imgs)
}

func (h ApiHandler) getAllTags(w http.ResponseWriter, r *http.Request) {
	tags, _ := h.service.GetAllTags()
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(tags)
}

func (h ApiHandler) deleteImage(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	id, _ := uuid.Parse(vars["id"])
	h.service.DeleteImage(id)
}

func (h ApiHandler) getImageMetadata(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	id, _ := uuid.Parse(vars["id"])
	img, _ := h.service.Get(id)
	if img != nil {
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(img)
	}
}

func (h ApiHandler) createImageMetadata(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	id, _ := uuid.Parse(vars["id"])

	reqBody, _ := ioutil.ReadAll(r.Body)
	var imgDto core.ImageDto
	json.Unmarshal(reqBody, &imgDto)

	img := h.service.SetImageMetadata(id, imgDto)
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(img)
}

func (h ApiHandler) downloadImage(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	id, _ := uuid.Parse(vars["id"])
	fileName := vars["fileName"]
	file, contentType := h.service.GetFileData(id)
	w.Header().Set("Content-Disposition", "attachment; filename="+fileName)
	w.Header().Set("Content-Type", contentType)
	// Copying the file content to response body
	w.Write(file)
}

func (h ApiHandler) uploadImage(w http.ResponseWriter, r *http.Request) {
	r.ParseMultipartForm(10 << 20)
	file, header, err := r.FormFile("file")

	if err != nil {
		fmt.Println(err)
		return
	}
	defer file.Close()
	fileBytes, err := ioutil.ReadAll(file)
	id := h.service.CreateImage(header.Filename, fileBytes, header.Header.Get("Content-Type"))
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(id.String())
}
