import React, { forwardRef, useImperativeHandle, useRef } from "react";
import { createPortal } from "react-dom";
import { CommonProps } from "../interfaces/CommonProps";
import { Dialog } from "../interfaces/Dialog";

const Modal = forwardRef(
  ({ children }: CommonProps, ref: React.Ref<Dialog>) => {
    const dialog = useRef<HTMLDialogElement>(null);

    useImperativeHandle(ref, () => {
      return {
        open: () => dialog.current!.showModal(),
        close: () => dialog.current!.close(),
      };
    });

    return createPortal(
      <dialog
        className="modal"
        ref={dialog}>
        {children}
      </dialog>,
      document.getElementById("modal")!
    );
  }
);

export default Modal;
