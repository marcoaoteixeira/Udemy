import { Place } from "../interfaces/Place";

const toRad = (value: number) => {
  return value * (Math.PI / 180);
};

const calculateDistance = (
  startLatitude: number,
  startLongitude: number,
  endLatitude: number,
  endLongitude: number
) => {
  const EARTH_RADIUS = 6371; // Radius of the Earth in kilometers
  const deltaRadiusLatitude = toRad(endLatitude - startLatitude);
  const deltaRadiusLongitude = toRad(endLongitude - startLongitude);
  const radiusStartLatitude = toRad(startLatitude);
  const radiusEndLatitude = toRad(endLatitude);

  const arc =
    Math.pow(Math.sin(deltaRadiusLatitude / 2), 2) +
    Math.cos(radiusStartLatitude) *
      Math.cos(radiusEndLatitude) *
      Math.pow(Math.sin(deltaRadiusLongitude / 2), 2);
  const circumference = 2 * Math.atan2(Math.sqrt(arc), Math.sqrt(1 - arc));
  const distance = EARTH_RADIUS * circumference;
  return distance;
};

export function sortPlacesByDistance(
  places: Place[],
  latitude: number,
  longitude: number
) {
  const sortedPlaces = [...places];
  sortedPlaces.sort((placeLeft, placeRight) => {
    const distanceLeft = calculateDistance(
      latitude,
      longitude,
      placeLeft.lat,
      placeLeft.lon
    );
    const distanceRight = calculateDistance(
      latitude,
      longitude,
      placeRight.lat,
      placeRight.lon
    );
    return distanceLeft - distanceRight;
  });
  return sortedPlaces;
}
