import React from "react";
import { CommonProps } from "../interfaces/CommonProps";
import { Place } from "../interfaces/Place";

export declare interface PlacesProps extends CommonProps {
  title: string;
  places: Place[];
  fallbackText?: string;
  onSelectPlace(id: string): void;
}

export default function Places({
  title,
  places,
  fallbackText,
  onSelectPlace,
}: PlacesProps): React.ReactElement {
  if (places.length === 0) {
    return <p className="fallback-text">{fallbackText}</p>;
  }

  return (
    <section className="places-category">
      <h2>{title}</h2>
      <ul className="places">
        {places.map((place) => (
          <li
            key={place.id}
            className="place-item">
            <button onClick={() => onSelectPlace(place.id)}>
              <img
                src={place.image.src}
                alt={place.image.alt}
              />
              <h3>{place.title}</h3>
            </button>
          </li>
        ))}
      </ul>
    </section>
  );
}
