import { Place } from "../interfaces/Place";
export default class LocalizationService {
  public sortPlacesByDistance(
    places: Place[],
    latitude: number,
    longitude: number
  ) {
    const result = [...places];
    result.sort((placeLeft, placeRight) => {
      const distanceLeft = this.calculateDistance(
        latitude,
        longitude,
        placeLeft.lat,
        placeLeft.lon
      );
      const distanceRight = this.calculateDistance(
        latitude,
        longitude,
        placeRight.lat,
        placeRight.lon
      );
      return distanceLeft - distanceRight;
    });
    return result;
  }

  private toRad(value: number) {
    return value * (Math.PI / 180);
  }

  private calculateDistance(
    startLatitude: number,
    startLongitude: number,
    endLatitude: number,
    endLongitude: number
  ) {
    const EARTH_RADIUS = 6371; // Radius of the Earth in kilometers
    const deltaRadiusLatitude = this.toRad(endLatitude - startLatitude);
    const deltaRadiusLongitude = this.toRad(endLongitude - startLongitude);
    const radiusStartLatitude = this.toRad(startLatitude);
    const radiusEndLatitude = this.toRad(endLatitude);

    const arc =
      Math.pow(Math.sin(deltaRadiusLatitude / 2), 2) +
      Math.cos(radiusStartLatitude) *
        Math.cos(radiusEndLatitude) *
        Math.pow(Math.sin(deltaRadiusLongitude / 2), 2);
    const circumference = 2 * Math.atan2(Math.sqrt(arc), Math.sqrt(1 - arc));
    const distance = EARTH_RADIUS * circumference;
    return distance;
  }
}
