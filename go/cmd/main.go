package main

import (
	"log"
	"net/http"

	api "github.com/nhart12/imageuploader/pkg/api"
	storage "github.com/nhart12/imageuploader/pkg/storage"
	"github.com/rs/cors"
)

func handleRequests() {
	apiHandler := api.NewRouter(storage.NewMockRepository())
	log.Fatal(http.ListenAndServe(":8081", cors.Default().Handler(apiHandler.Router)))
}

func main() {
	handleRequests()
}
