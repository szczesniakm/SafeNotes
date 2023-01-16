export type MessageType = "error" | "success" | "info";

export interface ToastMessage {
  message: string;
  type: MessageType;
}
