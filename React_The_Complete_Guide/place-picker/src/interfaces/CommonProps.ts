import { PropsWithRef, PropsWithoutRef } from "react";

export declare interface CommonProps {
  children?: React.ReactNode;
  style?: React.JSX.Element;
  props?: PropsWithoutRef<HTMLElement> | PropsWithRef<HTMLElement>;
}
