import React from "react";
import { CommonProps } from "../interfaces/CommonProps";

export declare interface DeleteConfirmationProps extends CommonProps {
  onConfirm?: React.FormEventHandler<HTMLButtonElement>;
  onCancel?: React.FormEventHandler<HTMLButtonElement>;
}

export default function DeleteConfirmation({
  onConfirm,
  onCancel,
}: DeleteConfirmationProps): React.ReactElement {
  return (
    <div id="delete-confirmation">
      <h2>Are you sure?</h2>
      <p>Do you really want to remove this place?</p>
      <div id="confirmation-actions">
        <button
          onClick={onCancel}
          className="button-text">
          No
        </button>
        <button
          onClick={onConfirm}
          className="button">
          Yes
        </button>
      </div>
    </div>
  );
}
