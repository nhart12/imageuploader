package main

import (
	"log"
	"net/http"

	api "github.com/nhart12/imageuploader/go/pkg/api"
	storage "github.com/nhart12/imageuploader/go/pkg/storage"
	"github.com/rs/cors"
)

func handleRequests() {
	apiHandler := api.NewRouter(storage.NewMockRepository())
	log.Fatal(http.ListenAndServe(":8081", cors.New(cors.Options{AllowedMethods: []string{"GET", "POST", "DELETE"}}).Handler(apiHandler.Router)))
}

func main() {
	handleRequests()
}
