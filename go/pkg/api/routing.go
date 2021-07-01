package api

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"

	"github.com/google/uuid"
	"github.com/gorilla/mux"
	"github.com/nhart12/imageuploader/pkg/core"
)

type ApiHandler struct {
	Router  http.Handler
	service ImageService
}

func NewRouter(repo core.Repository) ApiHandler {
	apiHandler := ApiHandler{service: NewImageService(repo)}
	httpRouter := mux.NewRouter().StrictSlash(true)

	httpRouter.HandleFunc("/api/images/{id}/metadata", apiHandler.createImageMetadata).Methods("POST")
	httpRouter.HandleFunc("/api/images/{id}", apiHandler.getImageMetadata).Methods("GET")
	httpRouter.HandleFunc("/api/images", apiHandler.uploadImage).Methods("POST")
	apiHandler.Router = httpRouter
	return apiHandler
}

func (h ApiHandler) getImageMetadata(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	id, _ := uuid.Parse(vars["id"])
	img, _ := h.service.Get(id)
	json.NewEncoder(w).Encode(img)
}

func (h ApiHandler) createImageMetadata(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	id, _ := uuid.Parse(vars["id"])

	reqBody, _ := ioutil.ReadAll(r.Body)
	var imgDto core.ImageDto
	json.Unmarshal(reqBody, &imgDto)

	h.service.SetImageMetadata(id, imgDto)
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
	id := h.service.CreateImage(header.Filename, fileBytes)
	fmt.Fprintf(w, id.String())

}
