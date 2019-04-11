export class NodeResponse {
id: number;
name: string;
nodeType: string;
latitude: number;
longitude: number;
points: number;
password : string;
  childNodeIds: number[];
  children: NodeResponse[];
  action: string;
  delay: number;
  image: any;
  duration: number;
  delta: number;
  bonus?: number;
  hint?: string;
  question?: string;
  answer?: string;
  canOverride?: boolean;
}
