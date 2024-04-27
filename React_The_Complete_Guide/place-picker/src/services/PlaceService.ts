import { Place } from "../interfaces/Place";
import { PlaceDataStore } from "../data/PlaceDataStore";

export default class PlaceService {
  private SELECTED_PLACES_KEY = "SELECTED_PLACES";

  public getAvailablePaces(): Place[] {
    return PlaceDataStore.AvailablePlaces;
  }

  public getSelectedPlaces(): Place[] {
    const stored = localStorage.getItem(this.SELECTED_PLACES_KEY) || "[]";
    const ids: string[] = JSON.parse(stored) || [];

    return ids.map(
      (id) =>
        PlaceDataStore.AvailablePlaces.find((place) => place.id === id) as Place
    );
  }

  public saveSelectedPlace(id: string): void {
    const stored = localStorage.getItem(this.SELECTED_PLACES_KEY) || "[]";
    const ids: string[] = JSON.parse(stored) || [];
    if (ids.indexOf(id) === -1) {
      ids.push(id);
      localStorage.setItem(this.SELECTED_PLACES_KEY, JSON.stringify(ids));
    }
  }

  public removeSelectedPlace(id: string): void {
    const stored = localStorage.getItem(this.SELECTED_PLACES_KEY) || "[]";
    const ids: string[] = JSON.parse(stored) || [];
    const index = ids.indexOf(id);
    if (index > -1) {
      ids.splice(index, 1);
      localStorage.setItem(this.SELECTED_PLACES_KEY, JSON.stringify(ids));
    }
  }
}
