import React, { Fragment, useEffect, useRef, useState } from "react";
import Modal from "./components/Modal";
import { Dialog } from "./interfaces/Dialog";
import DeleteConfirmation from "./components/DeleteConfirmation";
import placePickerLogo from "./assets/logo.png";
import Places from "./components/Places";
import { Place } from "./interfaces/Place";
import PlaceService from "./services/PlaceService.js";
import LocalizationService from "./services/LocalizationService.js";

const placeService = new PlaceService();
const localizationService = new LocalizationService();

const availablePlacesInitalState: Place[] = [];
const currentPlaceInitialState: Place = {
  id: "",
  title: "",
  image: {
    src: "",
    alt: "",
  },
  lat: 0,
  lon: 0,
};

export default function App(): React.ReactElement {
  const modal = useRef<Dialog>({
    open() {},
    close() {},
  });
  const currentPlace = useRef<Place>(currentPlaceInitialState);
  const [availablePlaces, setAvailablePlaces] = useState(
    availablePlacesInitalState
  );
  const [pickedPlaces, setPickedPlaces] = useState(
    placeService.getSelectedPlaces()
  );

  useEffect(() => {
    navigator.geolocation.getCurrentPosition((position) => {
      const places = localizationService.sortPlacesByDistance(
        placeService.getAvailablePaces(),
        position.coords.latitude,
        position.coords.longitude
      );
      setAvailablePlaces(places);
    });
  }, []);

  const confirmRemovePlaceHandler = () => {
    setPickedPlaces((prevState) => {
      placeService.removeSelectedPlace(currentPlace.current.id);

      return prevState.filter((place) => place.id !== currentPlace.current.id);
    });
    modal.current.close();
  };

  const cancelRemovePlaceHandler = () => {
    modal.current.close();
  };

  const removePlaceHandler = (id: string) => {
    modal.current.open();

    if (currentPlace.current !== undefined) {
      currentPlace.current.id = id;
    }
  };

  const selectPlaceHandler = (id: string) => {
    setPickedPlaces((prevState) => {
      if (prevState.some((place) => place.id === id)) {
        return prevState;
      }

      const place = availablePlaces.find((place) => place.id === id);
      if (place === undefined) {
        return prevState;
      }

      placeService.saveSelectedPlace(place.id);

      return [place, ...prevState];
    });
  };

  return (
    <Fragment>
      <Modal ref={modal}>
        <DeleteConfirmation
          onConfirm={confirmRemovePlaceHandler}
          onCancel={cancelRemovePlaceHandler}
        />
      </Modal>

      <header>
        <img
          src={placePickerLogo}
          alt="Stylized globe"
        />
        <h1>PlacePicker</h1>
        <p>
          Create your personal collection of places you would like to visit or
          you have visited.
        </p>
      </header>
      <main>
        <Places
          title="I'd like to visit..."
          fallbackText="Select the places you would like to visit below."
          places={pickedPlaces}
          onSelectPlace={removePlaceHandler}
        />
        <Places
          title="Available Places"
          places={availablePlaces}
          fallbackText="Sorting places by distance..."
          onSelectPlace={selectPlaceHandler}
        />
      </main>
    </Fragment>
  );
}
